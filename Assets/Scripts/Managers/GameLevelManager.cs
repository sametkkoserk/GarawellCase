using GameModels;
using UnityEngine;

public static class GameLevelManager
{
  private static LevelModel levelModel;

  public static LevelModel GetCurrentLevelModel()
  {
    return new LevelModel()
    {
      xStickCount = 6,
      yStickCount = 6,
      color = Color.cyan,
    };
  }
  public static int GetLevel()
  {
    if (!PlayerPrefs.HasKey(PlayerPrefsKeys.Level))
    {
      SetLevel(1);
    }
    return PlayerPrefs.GetInt(PlayerPrefsKeys.Level);
  }

  public static void SetLevel(int level)
  {
    PlayerPrefs.SetInt(PlayerPrefsKeys.Level,level);
  }

  public static void IncreaseLevel()
  {
    SetLevel(GetLevel()+1);
  }
}
