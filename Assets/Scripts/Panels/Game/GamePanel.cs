using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanel : MonoBehaviour
{

    public void OnClickRestartGame()
    {
        GamePhaseManager.instance.ChangePhase(GamePhase.Game);
    }
}
