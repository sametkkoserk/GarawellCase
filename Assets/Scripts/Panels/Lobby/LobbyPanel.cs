using System;
using System.Collections;
using System.Collections.Generic;
using Keys;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    private void Start()
    {
        levelText.text = $"LEVEL: {GameLevelManager.GetLevel()}";
    }

    public void OnClickPlay()
    {
        GamePhaseManager.instance.ChangePhase(GamePhase.Game);
    }

    public void OnClickSettings()
    {
        PanelsManager.instance.OpenPanel(PanelKeys.SettingsPanel,CanvasType.General);
    }
}
