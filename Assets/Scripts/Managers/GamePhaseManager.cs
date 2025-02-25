using System;
using Keys;
using UnityEngine;

public enum GamePhase
{
    None,
    Login,
    Lobby,
    Game,
}
public class GamePhaseManager : MonoBehaviour
{
    public static GamePhaseManager instance;
    public GamePhase currentPhase;
    public Action<GamePhase> OnGamePhaseChanged=delegate { };

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangePhase(GamePhase.Login);
    }

    public void ChangePhase(GamePhase newPhase)
    {
        currentPhase = newPhase;
        switch (currentPhase)
        {
            case GamePhase.Login:
                GameLevelManager.LoadLevelModel();
                ChangePhase(GamePhase.Lobby);
                break;
            case GamePhase.Lobby:
                PanelsManager.instance.ShowOnlyOneCanvas(CanvasType.Lobby);
                PanelsManager.instance.OpenPanel(PanelKeys.LobbyPanel,CanvasType.Lobby);

                break;
            case GamePhase.Game:
                PanelsManager.instance.ShowOnlyOneCanvas(CanvasType.Game);
                PanelsManager.instance.OpenPanel(PanelKeys.GamePanel,CanvasType.Game);
                break;
        }
        OnGamePhaseChanged.Invoke(currentPhase);
        
    }
    
}
