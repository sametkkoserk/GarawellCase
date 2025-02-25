using System;
using System.Collections.Generic;
using System.Linq;
using GameModels;
using UnityEngine;
using UnityEngine.EventSystems;

public class StickGroupController : MonoBehaviour
{
    public StickGroupModel stickGroup { get; private set; }

    public List<StickController> stickControllers { get; private set; }

    private void Awake()
    {
        stickControllers = new List<StickController>();
    }

    public void SetStickGroup(StickGroupModel model)
    {

        stickControllers.Clear();  
        
        stickGroup = model;
        float stickHeight = GridManager.instance.stickHeight;
        Debug.Log(JsonUtility.ToJson(stickGroup));
        
        int maxX = stickGroup.sticks.ConvertAll(item => item.direction == StickDirection.horizontal ?
            new StickModel(item.x+1,item.y,item.direction) : item).Max(item => item.x);
        int maxY = stickGroup.sticks.ConvertAll(item => item.direction == StickDirection.vertical ?
            new StickModel(item.x,item.y+ 1,item.direction) : item).Max(item => item.y);
        
        
        RectTransform rectTransform = GetComponent<RectTransform>(); 
        rectTransform.sizeDelta = new Vector2(stickHeight * maxX ,stickHeight * maxY);
        rectTransform.localPosition=Vector3.zero;
        
        for (int i = 0; i < stickGroup.sticks.Count; i++)
        {
            GameObject obj = PoolingManager.instance.GetObj(BundleKeys.StickController, transform);
            StickController stickController = obj.GetComponent<StickController>();
            Debug.Log(stickGroup.sticks[i].direction);
            Vector2 pos = new Vector2(stickGroup.sticks[i].x*stickHeight,stickGroup.sticks[i].y*stickHeight);
            stickController.SetPosition(stickGroup.sticks[i].x,stickGroup.sticks[i].y,stickGroup.sticks[i].direction,pos,stickHeight);
            stickControllers.Add(stickController);
        }
        
    }

    public void ResetStickGroup()
    {
        for (int i = 0; i < stickControllers.Count; i++)
        {
            PoolingManager.instance.ReturnObj(BundleKeys.StickController,stickControllers[i].gameObject);
        }
        PoolingManager.instance.ReturnObj(BundleKeys.StickGroupController,gameObject);    
    }

    private void OnDisable()
    {
        stickControllers.Clear();
        stickGroup = null;
    }
}