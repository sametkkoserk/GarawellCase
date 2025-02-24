using System.Collections;
using System.Collections.Generic;
using GameModels;
using UnityEngine;

[CreateAssetMenu(fileName = "StickGroups", menuName = "Scriptables/StickGroups")]
public class StickGroupsScriptable : ScriptableObject
{
  public List<StickGroupModel> stickGroups;

  public List<StickGroupModel> GetStickGroupsForCurrentLevel()
  {
    int currentLevel = GameLevelManager.GetLevel();
    return stickGroups.FindAll(item => item.minLevel <= currentLevel);
  }
}
