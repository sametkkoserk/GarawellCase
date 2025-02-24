using UnityEngine;


public static class PlayerInfoManager 
{
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
    }

}
