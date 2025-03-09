using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{
    public static PointerController instance;


    public Action OnGrabTile;
    public Action OnDropTile;
    
    private Camera m_camera;

    private StickGroupController stickGroupController;
    private HandStickGroupContainerController receivedContainer;

    private Vector2 lastMousePosition;
    private Vector2 targetPosition;
    private float speedMultiplier = 1.2f;
    private Vector2 dif;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!stickGroupController)return;
        
        if (Input.GetMouseButtonUp(0))
        {
            OnPointerDownUpStickGroup();
        }

        if (Input.GetMouseButton(0))
        {
            OnPointerMovingWithStickGroup();
        }
    }

    
    public void OnPointerDownOnStickGroup(StickGroupController _stickGroupController, Vector2 eventDataPosition, HandStickGroupContainerController handStickGroupContainerController)
    {
        if (stickGroupController) return;

        receivedContainer = handStickGroupContainerController;
        stickGroupController = _stickGroupController;
        stickGroupController.transform.parent = transform;

        lastMousePosition = Input.mousePosition;
        
        float referenceWidth = 1080f;
        float referenceHeight = 1920f;


        float referenceOffset = 225f;
        
        float scaleFactor = Mathf.Min(Screen.height / referenceHeight, Screen.width / referenceWidth);
        float yOffset = referenceOffset * scaleFactor;
        
        dif = new Vector2(stickGroupController.transform.position.x-lastMousePosition.x, yOffset- eventDataPosition.y);
        stickGroupController.transform.position = lastMousePosition + dif;
    }

    private void OnPointerMovingWithStickGroup()
    {
        if (!stickGroupController) return;

        GridManager.instance.isStickGroupCloseToAnySlotGroup(stickGroupController, GridStickState.Filling);

        Vector2 currentMousePosition = Input.mousePosition;
        Vector2 mouseDelta = currentMousePosition - lastMousePosition;

        targetPosition = (Vector2)stickGroupController.transform.position + (mouseDelta * speedMultiplier);

        stickGroupController.transform.position = targetPosition;

       
        lastMousePosition = currentMousePosition;
    }


    private void OnPointerDownUpStickGroup()
    {
        if (!stickGroupController)return;

        if (GridManager.instance.isStickGroupCloseToAnySlotGroup(stickGroupController, GridStickState.Filled))
        {
            receivedContainer.GroupReplaced();

            stickGroupController.ResetStickGroup();
            
        }
        else
        {
            receivedContainer.SetStickGroup(stickGroupController);
        }
        
        stickGroupController = null;
    }



}
