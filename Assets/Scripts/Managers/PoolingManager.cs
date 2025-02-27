using System;
using System.Collections;
using System.Collections.Generic;
using ManagerModels;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PoolingManager : MonoBehaviour
{
  public static PoolingManager instance;
  
  [SerializeField]
  private List<PoolItemModel> poolItems = new List<PoolItemModel>();
  private Dictionary<string, List<GameObject>> dict=new();
  
  private void Awake()
  {
    instance = this;
    
    for (int i = 0; i < poolItems.Count; i++)
    {
      int index = i;
      string key = poolItems[i].key.ToString();
      BundleManager.LoadBundle<GameObject>(key,(loadedObj =>
      {
        poolItems[index].loadedObj = loadedObj;
        dict[key] = new List<GameObject>();
        for (int j = 0; j < poolItems[index].count; j++)
        {
          GameObject obj = Instantiate(loadedObj, transform);
          obj.SetActive(false);
          dict[key].Add(obj);
        }
      }));
    }
  }

  public GameObject GetObj(BundleKeys bundleKey, Transform parent)
  {
    string key = bundleKey.ToString();
    if (!dict.ContainsKey(key))
    {
      Debug.LogError($"{key} is not pooled");
      return null;
    }

    if (dict[key].Count == 0)
    {
      GameObject loadedObj = poolItems.Find(item => item.key.ToString() == key).loadedObj;
      return Instantiate(loadedObj,parent);
    }

    GameObject obj=dict[key][0];
    obj.SetActive(true);
    obj.transform.parent = parent;
    dict[key].RemoveAt(0);
    return obj;
  }

  public void ReturnObj(BundleKeys bundleKey, GameObject obj)
  {
    string key = bundleKey.ToString();
    if (!dict.ContainsKey(key))
    {
      Destroy(obj);
      return;
    }
    obj.transform.parent = this.transform;
    obj.SetActive(false);
    dict[key].Add(obj);
  }
}


