using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameWonPanel : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI prizeText;
    private int prize => GameLevelManager.GetCurrentLevelModel().prize;
    private void Start()
    {
        prizeText.text=prize.ToString();
    }

    
    public void OnClickClaim()
    {
        PlayerInfoManager.AddGold(prize);
        GameLevelManager.IncreaseLevel();
        GamePhaseManager.instance.ChangePhase(GamePhase.Lobby);
        
    }
}
