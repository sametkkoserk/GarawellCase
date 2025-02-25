using GameModels;
using UnityEngine;

public static class GameLevelManager
{
  private static LevelModel levelModel;

  public static LevelModel GetCurrentLevelModel()
  {
    return levelModel;
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
    LoadLevelModel();
  }

  public static void IncreaseLevel()
  {
    SetLevel(GetLevel()+1);
  }

  public static void LoadLevelModel()
  {
    BundleManager.LoadBundle<LevelScriptable>($"LevelModel_{GetLevel()}",onComplete: (LevelScriptable) =>
    {
      levelModel=LevelScriptable.levelModel;
    });
  }
}
