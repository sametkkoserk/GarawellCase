using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Keys;
using UnityEngine;

public class HandController : MonoBehaviour
{
  public static HandController instance;
  [SerializeField] private List<HandStickGroupContainerController> containers;

  [SerializeField] private StickFactory stickFactory;

  private void Awake()
  {
    instance = this;
  }

  public void Start()
  {
    StartCoroutine(OnHandEmpty());
    for (int i = 0; i < containers.Count; i++)
    {
      containers[i].OnGroupPlaced += OnGroupPlaced;
    }
  }

  private void OnGroupPlaced()
  {
    ControlHandStickGroups();

    bool isAllEmpty = true;
    for (int i = 0; i < containers.Count; i++)
    {
      if (!containers[i].isEmpty)
      {
        isAllEmpty = false;
      }
    }

    if (isAllEmpty)
    {
      StartCoroutine(OnHandEmpty());
    }
  }

  public IEnumerator OnHandEmpty()
  {
    List<StickGroupController> stickGroupControllers = stickFactory.GetRandomStickGroups();
    for (int i = 0; i < stickGroupControllers.Count; i++)
    {
      containers[i].SetStickGroup(stickGroupControllers[i]);
      yield return new WaitForSeconds(0.1f);
    }
    yield return new WaitForSeconds(0.5f);
    ControlHandStickGroups();
  }

  private IEnumerator ControlHandStickGroups()
  {
    if (containers.FindAll(item=>!item.isEmpty).Count <= 0)yield break;
    for (int i = 0; i < containers.Count; i++)
    {
      if (containers[i].isEmpty)continue;

      if (GridManager.instance.isStickGroupPlacable(containers[i].stickGroupController))
      {
        yield break;
      }
    }

    yield return new WaitForEndOfFrame();
    PanelsManager.instance.OpenPanel(PanelKeys.GameLosePanel,CanvasType.Game);
  }

  public void ResetHand()
  {
    for (int i = 0; i < containers.Count; i++)
    {
      containers[i].ResetContainer();
    }

    StartCoroutine(OnHandEmpty());
  }

  private void OnDestroy()
  {
    for (int i = 0; i < containers.Count; i++)
    {
      containers[i].OnGroupPlaced -= OnGroupPlaced;
    }  
  }
}
