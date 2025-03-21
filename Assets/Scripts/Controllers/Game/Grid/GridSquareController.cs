﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum SquareSide
{
  Top,
  Bottom,
  Right,
  Left
}

public class GridSquareController : MonoBehaviour
{
  [SerializeField]
  private Image image;

  public bool isFilled = false;
  
  private int xPos;
  private int yPos;
  
  public Action<KeyValuePair<int,int>,bool> OnSquareValueChanged=delegate(KeyValuePair<int, int> b, bool arg2) {  };
  private Dictionary<SquareSide,GridStickController> sticks;
  
  private void OnEnable()
  {
    if (GameLevelManager.GetCurrentLevelModel()==null)return;

    image.transform.localScale=Vector3.zero;
    image.color = GameLevelManager.GetCurrentLevelModel().color;
    
  }
  public void SetPosition(int x, int y, float height, Dictionary<SquareSide, GridStickController> gridStickControllers)
  {
    xPos = x;
    yPos = y;
    sticks = gridStickControllers;
    Vector2 pos = new Vector2(x * height, y * height);
    RectTransform rectTransform = GetComponent<RectTransform>();
    rectTransform.sizeDelta = new Vector2(height, height);
    rectTransform.pivot=Vector2.zero;
    rectTransform.anchorMin=Vector2.zero;
    rectTransform.anchorMax=Vector2.zero;
    rectTransform.anchoredPosition = pos;

    for (int i = 0; i < sticks.Values.Count; i++)
    {
      sticks.ElementAt(i).Value.OnGridStickStateChanged += OnGridStickStateChanged;
    }
  }

  private void OnGridStickStateChanged(GridStickState obj)
  {
    if (isFilled)return;
    isFilled = true;
    for (int i = 0; i < sticks.Values.Count; i++)
    {
      if (!sticks.ElementAt(i).Value.isFilled)
        isFilled = false;
    }


    if (isFilled)
    {
      LeanTween.scale(image.gameObject, Vector3.one, 0.5f).setOnComplete((o =>
      {
        OnSquareValueChanged.Invoke(new KeyValuePair<int, int>(xPos,yPos),isFilled);
      }));
    }


    
  }

  public void SquareBlasted(List<KeyValuePair<int, int>> exceptList)
  {
    if (!isFilled)return;
    for (int i = 0; i < sticks.Count; i++)
    {
      bool isStickExcept = false;
      for (int j = 0; j < exceptList.Count; j++)
      {
        if (sticks.ElementAt(i).Value.isInIndexedSquare(exceptList[j]))
        {
          isStickExcept = true;
        }
      }
      if (!isStickExcept)
      {
        sticks.ElementAt(i).Value.StickBlasted();
      }
    }

    LeanTween.scale(image.gameObject, Vector3.zero, 0.5f);
    isFilled = false;
  }

  private void OnDisable()
  {
    if (sticks == null)return;

    for (int i = 0; i < sticks.Values.Count; i++)
    {
      sticks.ElementAt(i).Value.OnGridStickStateChanged -= OnGridStickStateChanged;
    }
    isFilled = false;
    sticks.Clear();
  }
}


