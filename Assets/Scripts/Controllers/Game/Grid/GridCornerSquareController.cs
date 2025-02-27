using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCornerSquareController : MonoBehaviour
{
  [SerializeField] private Image image;
  [SerializeField] private List<Color> colors = new List<Color>();
  
  private List<GridStickController> sticks;
  

  private void OnEnable()
  {
    if (GameLevelManager.GetCurrentLevelModel()==null)return;

    SetColors();
  }

  public void SetPosition(Vector2 pos, float height, List<GridStickController> stickList)
  {
    sticks = stickList;
    
    RectTransform rectTransform = GetComponent<RectTransform>();
    rectTransform.sizeDelta = new Vector2(height/2.5f, height/2.5f);
    rectTransform.anchorMin=Vector2.zero;
    rectTransform.anchorMax=Vector2.zero;
    rectTransform.anchoredPosition = pos;

    for (int i = 0; i < sticks.Count; i++)
    {
      sticks[i].OnGridStickStateChanged += OnStateChange;
    }
    OnStateChange(GridStickState.Empty);
  }

  private void SetColors()
  {
    colors.Add(GameConfigManager.instance.gameDefaultSettings.emptyStickColor);
    Color levelColor = GameLevelManager.GetCurrentLevelModel().color;
    colors.Add(new Color(Mathf.Max(levelColor.r-0.1f,0),Mathf.Max(levelColor.g-0.1f,0),Mathf.Max(levelColor.b-0.1f,0)));
    colors.Add(levelColor);  
  }

  private void OnStateChange(GridStickState state)
  {
    bool isFilled=false;
    bool isFilling = false;
    for (int i = 0; i < sticks.Count; i++)
    {
      if (sticks[i].isFilled)
        isFilled = true;
      else if (sticks[i].isFilling)
        isFilling = true;
    }

    image.color = colors[isFilled ? 2 : isFilling ? 1 : 0];
  }

  private void OnDisable()
  {
    colors.Clear();

    if (sticks==null) return;

    for (int i = 0; i < sticks.Count; i++)
    {
      sticks[i].OnGridStickStateChanged -= OnStateChange;
    }
    sticks.Clear();

  }
}
