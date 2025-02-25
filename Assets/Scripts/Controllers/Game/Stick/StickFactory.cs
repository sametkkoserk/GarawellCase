using System;
using System.Collections;
using System.Collections.Generic;
using GameModels;
using UnityEngine;
using Random = System.Random;

public class StickFactory : MonoBehaviour
{
  [SerializeField]private StickGroupsScriptable stickGroupsScriptable;
  [SerializeField]private List<Transform> containers = new List<Transform>();
  
  private List<StickGroupModel> stickGroups;
  Random rnd = new Random();


  private void Awake()
  {
    stickGroups = stickGroupsScriptable.GetStickGroupsForCurrentLevel();
  }

  private void Start()
  {
  }

  public List<StickGroupController> GetRandomStickGroups()
  {
    List<StickGroupController> stickGroupControllers=new List<StickGroupController>();
    for (int i = 0; i < containers.Count; i++)
    {
      int index = GetRandomIndex(stickGroups.Count);
      StickGroupModel stickGroupModel = stickGroups[index];
      GameObject groupObj=PoolingManager.instance.GetObj(BundleKeys.StickGroupController, containers[i]);
      StickGroupController stickGroupController = groupObj.GetComponent<StickGroupController>();
      stickGroupControllers.Add(stickGroupController);
      stickGroupController.SetStickGroup(stickGroupModel);
      stickGroupController.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    return stickGroupControllers;
  }

  private int GetRandomIndex(int size)
  {
    int x=rnd.Next(size);
    Debug.Log(x);
    if (size==x)
    {
      Debug.LogError("Sıkıntı var");
    }

    return x;
  }
}
