using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandStickGroupContainerController : MonoBehaviour , IPointerDownHandler
{
  public StickGroupController stickGroupController{get; private set;}

  private float scale = 0.75f;
  public bool isEmpty => stickGroupController == null;
  public Action OnGroupPlaced=delegate {  };
  public void SetStickGroup(StickGroupController _stickGroupController, bool withAnim=false)
  {
    if (stickGroupController)return;
    _stickGroupController.transform.parent = transform;

    if (withAnim)
      SetStickGroupWithAnim(_stickGroupController);
    else
      SetStickGroupWithoutAnim(_stickGroupController);

  }

  public void SetStickGroupWithAnim(StickGroupController _stickGroupController)
  {
    LeanTween.scale(_stickGroupController.gameObject, _stickGroupController.isGroupLong? new Vector3(scale, scale, scale) : Vector3.one, 0.2f).setEaseInCirc();
    LeanTween.moveLocal(_stickGroupController.gameObject, Vector3.zero, 0.4f).setEaseInCirc().setOnComplete(obj =>
    {
      stickGroupController = _stickGroupController;
    });
  }
  public void SetStickGroupWithoutAnim(StickGroupController _stickGroupController)
  {
    stickGroupController = _stickGroupController;
    
    stickGroupController.transform.localScale = stickGroupController.isGroupLong? new Vector3(scale, scale, scale) : Vector3.one;
    stickGroupController.transform.localPosition=Vector3.zero;
  }
  
  public void OnPointerDown(PointerEventData eventData)
  {
    if (!stickGroupController)return;
    AudioManager.instance.PlaySfx("OnPickAudio");
    stickGroupController.transform.localScale=Vector3.one;
    PointerController.instance.OnPointerDownOnStickGroup(stickGroupController, (eventData.position - (Vector2)transform.position),this);
    stickGroupController = null;
  }

  public void GroupReplaced()
  {
    AudioManager.instance.PlaySfx("OnPlacedAudio");
    OnGroupPlaced.Invoke();
  }

  public void ResetContainer()
  {
    if (stickGroupController == null)return;
    
    stickGroupController.ResetStickGroup();
    stickGroupController = null;
  }
}
