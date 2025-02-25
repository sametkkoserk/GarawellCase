using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickController : MonoBehaviour
{
  
  [SerializeField]
  protected Image image;
  
  protected int xPos;
  protected int yPos;
  protected StickDirection direction;

  public virtual void SetPosition(int x, int y, StickDirection stickDirection, Vector2 pos,float height)
  {
    xPos = x;
    yPos = y;
    direction=stickDirection;
    

    RectTransform rectTransform = GetComponent<RectTransform>();
    rectTransform.pivot=new Vector2(0.5f ,0.5f);
    image.GetComponent<RectTransform>().sizeDelta = new Vector2(height / 3, height +(height/3));
    image.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
    
    rectTransform.sizeDelta = new Vector2(height / 5, height);
    rectTransform.pivot=new Vector2(0.5f ,0);
    rectTransform.anchorMin=Vector2.zero;
    rectTransform.anchorMax=Vector2.zero;
    rectTransform.anchoredPosition = pos;
    
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
