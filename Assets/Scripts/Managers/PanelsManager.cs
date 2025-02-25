using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum CanvasType
{
    General,
    Game,
    Lobby,
    None,
}
public class PanelsManager : MonoBehaviour
{
    public static PanelsManager instance;
    Dictionary<CanvasType,Transform> canvases=new Dictionary<CanvasType,Transform>();
    private Dictionary<CanvasType, Dictionary<string, GameObject>> panels =
        new Dictionary<CanvasType, Dictionary<string, GameObject>>();
    private AsyncOperationHandle<GameObject> asyncOperationHandle;
    private List<string> lastOpenedPanel = new List<string>();

    private void Awake()
    {
        instance = this;
        List<PanelsManagerCanvasController> canvasControllers=GetComponentsInChildren<PanelsManagerCanvasController>().ToList();
        for (int i = 0; i < canvasControllers.Count; i++)
        {
            canvases.Add(canvasControllers[i].canvasType,canvasControllers[i].transform);
            panels.Add(canvasControllers[i].canvasType,new Dictionary<string, GameObject>());
        }
    }
    public void OpenPanel(string key, CanvasType canvas,OnPanelInstantiated callback=null)
    {
        Dictionary<string, GameObject> panels = new();
        try
        {
            callback += handle =>
            {
                lastOpenedPanel.Add(key);
            };
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        InstantiatePanel(key,canvas,callback);
    }

    public void ClosePanel(string key, CanvasType canvas)
    {
        Dictionary<string, GameObject> panels = new();
        DestroyPanel(key,canvas);
        
    }

    public void ShowOnlyOneCanvas(CanvasType canvas)
    {
        CloseAllCanvases();

        for (int i = 0; i < canvases.Count; i++)
        {
            if (canvas == canvases.ElementAt(i).Key)
                canvases.ElementAt(i).Value.gameObject.SetActive(true);
            else
                ClearCanvas(canvas);
        }
    }

    private void ClearCanvas(CanvasType canvas)
    {
        for (int i = 0; i < panels[canvas].Values.Count; i++)
        {
            DestroyPanel(panels[canvas].ElementAt(i).Key,canvas);
        }
    }

    public void CloseAllCanvases()
    {
        for (int i = 0; i < canvases.Count; i++)
        {
            canvases.ElementAt(i).Value.gameObject.SetActive(false);
        }
    }
    
    private void InstantiatePanel(string key, CanvasType canvas, OnPanelInstantiated callback)
    {
        if (panels[canvas].ContainsKey(key))
        {
            Debug.LogWarning(key + " already Opened. ");
            return;
        }

        panels[canvas][key] = null;

        asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(key);

        asyncOperationHandle.Completed +=
            (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded && panels[canvas].ContainsKey(key))
                {
                    var obj = Instantiate(handle.Result, canvases[canvas]);

                    panels[canvas][key] = obj;
                    if (callback != null)
                    {
                        callback.Invoke(obj);
                    }
                }
                else
                {
                    Debug.Log("Failed To Load " + key + ".");
                }
            };
    }

    private void DestroyPanel(string key, CanvasType canvas)
    {
        
        if (panels[canvas].ContainsKey(key))
        {
            if (panels[canvas][key] == null)
            {
                panels[canvas].Remove(key);

                if (lastOpenedPanel.Count != 0 && key == lastOpenedPanel.Last())
                {
                    asyncOperationHandle.Completed += (handle) =>
                    {
                        if (handle.Status == AsyncOperationStatus.Succeeded)
                        {
                            Destroy(handle.Result);

                            if (lastOpenedPanel.Contains(key))
                                lastOpenedPanel.Remove(key);
                        }
                    };
                }

            }
            else
            {
                if (lastOpenedPanel.Contains(key))
                    lastOpenedPanel.Remove(key);

                Destroy(panels[canvas][key]);
                panels[canvas].Remove(key);
            }


            Debug.Log($"Panel {key} Destroyed.");
        }

    }
    public delegate void OnPanelInstantiated(GameObject obj);
}
