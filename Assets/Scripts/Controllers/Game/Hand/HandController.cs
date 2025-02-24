using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
  [SerializeField] private List<HandStickGroupContainerController> containers;

  [SerializeField] private StickFactory stickFactory;

  public void Start()
  {
    StartCoroutine(OnHandEmpty());
    for (int i = 0; i < containers.Count; i++)
    {
      containers[i].OnGroupReplaced += OnGroupPlaced;
    }
  }

  private void OnGroupPlaced()
  {
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
  }
}
