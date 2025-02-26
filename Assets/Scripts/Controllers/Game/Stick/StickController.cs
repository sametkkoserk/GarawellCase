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
  
  protected int xPos;
  protected int yPos;
  protected StickDirection direction;
  protected float stickHeight;

  private StickModel stickModel;



  public void SetPosition(StickModel _stickModel,float height)
  {
    stickModel = _stickModel;
    
    xPos = stickModel.x;
    yPos = stickModel.y;
    direction=stickModel.direction;
    stickHeight = height;
    
    SetStick();
    StartCoroutine(SetStickImage());

  }

  private IEnumerator SetStickImage()
  {
    yield return new WaitForEndOfFrame();
    if (stickModel.sprite)
    {
      image.sprite = stickModel.sprite;
    }
  }


  protected void SetStick()
  {
    Vector2 pos = new Vector2(xPos*stickHeight,yPos*stickHeight);

    RectTransform rectTransform = GetComponent<RectTransform>();
    rectTransform.pivot=new Vector2(1f ,0.5f);
    image.GetComponent<RectTransform>().sizeDelta = new Vector2(1.33f*stickHeight/3.085f , 1.33f*stickHeight );
    rectTransform.pivot=new Vector2(0.5f ,0.5f);
    image.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;

    rectTransform.sizeDelta = new Vector2(stickHeight / 5, stickHeight);
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

  private void OnDisable()
  {
    gameObject.transform.localScale=Vector3.one;
  }
}
