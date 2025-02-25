using System.Collections;
using System.Collections.Generic;
using GameModels;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptables/Level")]
public class LevelScriptable : ScriptableObject
{
  public LevelModel levelModel;
}
