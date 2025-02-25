using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerController : MonoBehaviour
{
    public static PointerController instance;


    public Action OnGrabTile;
    public Action OnDropTile;
    
    private Camera m_camera;

    private StickGroupController stickGroupController;
    private HandStickGroupContainerController receivedContainer;
    private Vector2 dif ;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!stickGroupController)return;
        
        if (Input.GetMouseButtonUp(0))
        {
            OnPointerDownOnUpStickGroup();
        }

        if (Input.GetMouseButton(0))
        {
            Debug.Log("MouseClicked");
            OnPointerMovingWithStickGroup();
        }
    }

    public void OnPointerDownOnStickGroup(StickGroupController _stickGroupController, Vector2 eventDataPosition, HandStickGroupContainerController handStickGroupContainerController)
    {
        if (stickGroupController)return;

        receivedContainer = handStickGroupContainerController;
        stickGroupController = _stickGroupController;
        
        stickGroupController.transform.parent = transform;
        dif = new Vector2(0, 200 - eventDataPosition.y);
        Vector2 groupPos = (Vector2)Input.mousePosition + dif;
        stickGroupController.transform.position = groupPos;

    }
    
    private void OnPointerMovingWithStickGroup()
    {
        if (!stickGroupController)return;
        Debug.Log("MouseMovement"+Input.GetAxis("Mouse X")+"-"+Input.GetAxis("Mouse Y"));
        GridManager.instance.isStickGroupCloseToAnySlotGroup(stickGroupController,GridStickState.Filling);
        stickGroupController.transform.position = (Vector2)Input.mousePosition + dif;
        
    }

    private void OnPointerDownOnUpStickGroup()
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
