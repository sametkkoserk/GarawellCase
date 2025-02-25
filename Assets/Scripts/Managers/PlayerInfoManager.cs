using System;
using UnityEngine;


public static class PlayerInfoManager 
{
    public static Action<int> OnGoldUpdated=delegate { };
    public static int GetGold()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKeys.Gold))
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.Gold,500);

        }
        return PlayerPrefs.GetInt(PlayerPrefsKeys.Gold);
    }

    public static void SetGold(int gold)
    {
        PlayerPrefs.SetInt(PlayerPrefsKeys.Gold,gold);
        OnGoldUpdated.Invoke(gold);
    }

    public static void AddGold(int gold)
    {
        SetGold(GetGold()+gold);
    }

}
