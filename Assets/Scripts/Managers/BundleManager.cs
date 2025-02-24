using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class BundleManager
{
  public static void LoadBundle<T>(string key, Action<T> onComplete)
  {
    Debug.Log("KEY:"+key);
    Addressables.LoadAssetAsync<T>(key).Completed += handle =>
    {
      if (handle.Status == AsyncOperationStatus.Succeeded)
      {
        Debug.Log(handle.Result);
        
        onComplete.Invoke(handle.Result);
      }
    };
  }
}
