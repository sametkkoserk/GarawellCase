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
    
    Application.targetFrameRate = 60;
    Application.runInBackground = true;
  }
}
