using System;
using UnityEngine;
using UnityEngine.UI;

public enum GridStickState
{
  None,
  Empty,
  Filling,
  Filled
}

public class GridStickController : StickController
{
  public bool isFilled=>state==GridStickState.Filled;
  public bool isFilling => state == GridStickState.Filling;
  
  private GridStickState state = GridStickState.None;
  public Action<GridStickState> OnGridStickStateChanged = delegate(GridStickState stickState) { };
  
  private void Awake()
  {
    GetComponent<Image>().color = GameConfigManager.instance.gameDefaultSettings.emptyStickColor;
    ChangeState(GridStickState.Empty);
  }
  public virtual void SetPosition(int x, int y, StickDirection stickDirection,float height)
  {
    xPos = x;
    yPos = y;
    direction=stickDirection;
    stickHeight = height;
    SetStick();

  }

  protected override void SetStick()
  {
    Vector2 pos = new (xPos*stickHeight,yPos*stickHeight);

    image.rectTransform.pivot=new Vector2(0.5f ,0.5f);
    image.rectTransform.localPosition = Vector3.zero;
    image.GetComponent<RectTransform>().sizeDelta = new Vector2( stickHeight/3 , 1.33f*stickHeight );
      

    rectTransform.sizeDelta = new Vector2(stickHeight / 5, stickHeight);
    rectTransform.pivot=new Vector2(0.5f ,0);
    rectTransform.anchorMin=Vector2.zero;
    rectTransform.anchorMax=Vector2.zero;
    rectTransform.anchoredPosition = pos;
    
    base.SetStick();
  }
  public void ChangeState(GridStickState _state)
  {
    if (state==_state)return;

    state = _state;
    image.gameObject.SetActive(state!=GridStickState.Empty);

    switch (state)
    {
      case GridStickState.Empty:
        break;
      case GridStickState.Filling:
        image.color = new Color(255,255,255, 100);
        break;
      case GridStickState.Filled:
        image.color = GameLevelManager.GetCurrentLevelModel().color;
        break;
    }
    OnGridStickStateChanged.Invoke(state);
  }

  public void StickBlasted()
  {
    if (!isFilled)return;
    
    ChangeState(GridStickState.Empty);

  }

  private void OnDisable()
  {
    ChangeState(GridStickState.Empty);
    rectTransform.pivot = new Vector2(0.5f, 0.5f);
  }
  
}

