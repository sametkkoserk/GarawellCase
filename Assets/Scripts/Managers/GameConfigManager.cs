using System;
using System.Collections;
using System.Collections.Generic;
using GameModels;
using UnityEngine;

public class GameConfigManager : MonoBehaviour
{
  public static GameConfigManager instance;

  [SerializeField]
  public GameDefaultSettings gameDefaultSettings;
  private void Awake()
  {
    instance = this;
#if UNITY_EDITOR
    Debug.unityLogger.logEnabled = true;
#else
    Debug.unityLogger.logEnabled = false;
#endif
    Application.targetFrameRate = 60;
    Application.runInBackground = true;
  }
}
