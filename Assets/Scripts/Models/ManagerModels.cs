using System;
using UnityEngine;

namespace ManagerModels
{
  [Serializable]
  public class PoolItemModel
  {
    public BundleKeys key;
    public int count;
    [NonSerialized]
    public GameObject loadedObj;
  }
}
