using System;
using System.Collections;
using System.Collections.Generic;
using GameModels;
using Keys;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointTargetController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private Slider slider;
    TargetModel targetModel => GameLevelManager.GetCurrentLevelModel().targets.Find(item => item.targetType == TargetType.Point);
    private int currentPoint = 0;
    private void Start()
    {
        if (targetModel == null)
        {
            gameObject.SetActive(false);
            return;
        }
        targetText.text = targetModel.targetAmount.ToString();
        GridManager.instance.OnBlasted += OnBlast;
        ReactionController.instance.onComboMade += OnComboMade;
    }

    private void OnComboMade(int count)
    {
        currentPoint+=count*10;
        slider.value = (float)currentPoint/(float)targetModel.targetAmount;
        if (targetModel.targetAmount<currentPoint)
            PanelsManager.instance.OpenPanel(PanelKeys.GameWonPanel,CanvasType.Game);
    }
    private void OnBlast(int count)
    {
        Debug.Log(count);
        currentPoint+=count*count*2*ReactionController.instance.comboCounter;
        slider.value = (float)currentPoint/(float)targetModel.targetAmount;
        if (targetModel.targetAmount<currentPoint)
            PanelsManager.instance.OpenPanel(PanelKeys.GameWonPanel,CanvasType.Game);
    }

    private void OnDestroy()
    {
        GridManager.instance.OnBlasted -= OnBlast;
        ReactionController.instance.onComboMade -= OnComboMade;

    }
}
