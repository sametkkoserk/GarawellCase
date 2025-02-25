using System.Collections;
using System.Collections.Generic;
using Keys;
using TMPro;
using UnityEngine;

public class GameLosePanel : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI rewievAmountText;
    private int prize => GameLevelManager.GetCurrentLevelModel().prize;
    private void Start()
    {
        rewievAmountText.text=((int)(prize/2)).ToString();
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
