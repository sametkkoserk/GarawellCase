using System.Collections;
using System.Collections.Generic;
using Keys;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLosePanel : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI reviveAmountText;
    [SerializeField]private Button reviveButton;

    private int prize => GameLevelManager.GetCurrentLevelModel().prize;
    
    private void Start()
    {
        reviveButton.interactable = PlayerInfoManager.GetGold() >= (int)(prize / 2);
        reviveAmountText.text=((int)(prize/2)).ToString();
    }


    public void OnClickRewiev()
    {
        PlayerInfoManager.AddGold(-((int)(prize/2)));
        HandController.instance.ResetHand();
        PanelsManager.instance.ClosePanel(PanelKeys.GameLosePanel,CanvasType.Game);
    }
    public void OnClickLose()
    {
        GamePhaseManager.instance.ChangePhase(GamePhase.Lobby);
        
    }
}
