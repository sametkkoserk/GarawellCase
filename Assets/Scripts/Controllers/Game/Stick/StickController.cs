using System;
using System.Collections;
using System.Collections.Generic;
using GameModels;
using UnityEngine;
using UnityEngine.UI;

public class StickController : MonoBehaviour
{
  
  [SerializeField]
  protected Image image;
  protected RectTransform rectTransform => GetComponent<RectTransform>();
  public int xPos { get; protected set; }
  public int yPos{ get; protected set; }
  public StickDirection direction{ get; protected set; }
  protected float stickHeight;
  
  protected virtual void SetStick()
  {
    image.color = GameLevelManager.GetCurrentLevelModel().color;

    rectTransform.localRotation=Quaternion.Euler(new Vector3(0,0,direction==StickDirection.vertical?0:-90));
  }

  public bool isInIndexedSquare(KeyValuePair<int,int> pos)
  {
    if (direction==StickDirection.vertical)
    {
      return (pos.Key == xPos && pos.Value == yPos) || (pos.Key+1 == xPos && pos.Value == yPos);
    }
    else
    {
      return (pos.Key == xPos && pos.Value == yPos) || (pos.Key == xPos && pos.Value+1 == yPos);
    }
  }
  

}
