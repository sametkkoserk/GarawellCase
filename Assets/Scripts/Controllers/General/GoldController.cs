using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GoldController : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    void Start()
    {
        goldText.text = PlayerInfoManager.GetGold().ToString();
        PlayerInfoManager.OnGoldUpdated += OnGoldUpdated;

    }

    private void OnGoldUpdated(int gold)
    {
        goldText.text = gold.ToString();
    }

    private void OnDestroy()
    {
        PlayerInfoManager.OnGoldUpdated -= OnGoldUpdated;
    }
}
