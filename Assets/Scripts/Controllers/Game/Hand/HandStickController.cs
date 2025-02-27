using System;
using System.Collections;
using GameModels;
using UnityEngine;

public class HandStickController : StickController
{
  private StickModel stickModel;

  private float pivotPoint => stickModel.spriteModel.pivotPoint;
  public Vector3 comparisonPosition => transform.position + new Vector3(direction==StickDirection.vertical?dif:stickHeight * 0.05f, direction==StickDirection.vertical?stickHeight * 0.05f:dif, 0);

  private float dif => (pivotPoint == 0.5f ? 0 : pivotPoint == 0 ? +1 : -1) * (scaleMult * stickHeight / 3.085f / 2);

  private float scaleMult = 1.1f;
  private void Update()
  {
    if (direction==StickDirection.horizontal)
    {
      Debug.Log($"{pivotPoint}:{transform.position}---{comparisonPosition}");

    }
  }

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
  protected override void SetStick()
  {
    Vector2 pos = new (xPos*stickHeight*scaleMult,yPos*stickHeight*scaleMult);

    rectTransform.sizeDelta = new Vector2(scaleMult*stickHeight/ 3.085f, stickHeight*scaleMult);
    rectTransform.pivot=new Vector2(stickModel.spriteModel.pivotPoint ,0);
    rectTransform.anchorMin=Vector2.zero;
    rectTransform.anchorMax=Vector2.zero;
    rectTransform.anchoredPosition = pos;
    
    base.SetStick();
  }
  private IEnumerator SetStickImage()
  {
    yield return new WaitForEndOfFrame();
    if (stickModel.spriteModel.sprite)
    {
      image.sprite = stickModel.spriteModel.sprite;
    }
  }
  private void OnDisable()
  {
    gameObject.transform.localScale=Vector3.one;
  }
}
