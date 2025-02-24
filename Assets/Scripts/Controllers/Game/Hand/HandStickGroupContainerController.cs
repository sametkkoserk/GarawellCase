using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandStickGroupContainerController : MonoBehaviour , IPointerDownHandler
{
  private StickGroupController stickGroupController;

  public bool isEmpty => stickGroupController == null;
  public Action OnGroupReplaced=delegate {  };
  public void SetStickGroup(StickGroupController _stickGroupController)
  {
    if (stickGroupController)return;
    _stickGroupController.transform.parent = transform;

    LeanTween.moveLocal(_stickGroupController.gameObject, Vector3.zero, 0.4f).setEaseInCirc().setOnComplete(obj =>
    {
      stickGroupController = _stickGroupController;
    });

  }
  public void OnPointerDown(PointerEventData eventData)
  {
    if (!stickGroupController)return;

    PointerController.instance.OnPointerDownOnStickGroup(stickGroupController, (eventData.position - (Vector2)transform.position),this);
    stickGroupController = null;
  }

  public void GroupReplaced()
  {
    OnGroupReplaced.Invoke();
  }
}
