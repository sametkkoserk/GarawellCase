using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameModels;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    private Dictionary<KeyValuePair<int, int>, GridCornerSquareController> cornerSquares = new();
    private Dictionary<KeyValuePair<int, int>, GridSquareController> squares = new();
    private Dictionary<StickDirection, Dictionary<KeyValuePair<int, int>, GridStickController>> sticks = new(2);

    List<GridStickController> nearestGridStickControllers = new List<GridStickController>();

    public float stickHeight { get; private set; }
    private LevelModel levelModel; 
    
    public Action<int> OnBlasted=delegate { };
    private bool isSquareMade = false;
    private void Awake()
    {
        instance = this;
    }

    public void SetGridInfo(LevelModel _levelModel, float _stickHeight,
        Dictionary<KeyValuePair<int, int>, GridSquareController> _squares,
        Dictionary<KeyValuePair<int, int>, GridStickController> _verticalSticks,
        Dictionary<KeyValuePair<int, int>, GridStickController> _horizontalSticks,
        Dictionary<KeyValuePair<int, int>, GridCornerSquareController> _cornerSquares)
    {
        levelModel = _levelModel;
        stickHeight = _stickHeight;
        squares = _squares;
        sticks.Add(StickDirection.vertical,_verticalSticks);
        sticks.Add(StickDirection.horizontal,_horizontalSticks);
        for (int i = 0; i < squares.Count; i++)
        {
            squares.ElementAt(i).Value.OnSquareValueChanged += OnSquareValueChanged;
        }

        cornerSquares = _cornerSquares;
        Debug.Log("Grid Initialized");
    }

    
    private void OnSquareValueChanged(KeyValuePair<int, int> pos, bool isFilled)
    {
        if (isFilled)
            isSquareMade = true;

        Dictionary<KeyValuePair<int,int>,GridSquareController> completeLines = GetBlastableSquares(true);
        List<KeyValuePair<int,int>> exceptList=GetBlastableSquares(false).Keys.ToList();
        
        for (int i = 0; i < completeLines.Count; i++)
        {
            completeLines.ElementAt(i).Value.SquareBlasted(exceptList);
        }

        if (completeLines.Count>0)
        {
            OnBlasted.Invoke(completeLines.Count);
            AudioManager.instance.PlaySfx("OnBlastAudio");
        }
    }

    private Dictionary<KeyValuePair<int,int>,GridSquareController> GetBlastableSquares(bool isBlastable)
    {
        Dictionary<KeyValuePair<int,int>,GridSquareController> gridSquareControllers = new();
        
        for (int i = 0; i <  levelModel.xStickCount; i++)
        {
            for (int j = 0; j < levelModel.yStickCount; j++)
            {
                KeyValuePair<int, int> index = new KeyValuePair<int, int>(i,j);
                if (squares[index].isFilled)
                {
                    if (ControlXAndYLines(index)==isBlastable)
                    {
                        gridSquareControllers.Add(index,squares[index]);
                    }
                }
            }
        }

        return gridSquareControllers;
    }
    
    private bool ControlXAndYLines(KeyValuePair<int, int> index)
    {
        bool isFilledX = true;
        bool isFilledY = true;

        for (int i = 0; i < levelModel.xStickCount; i++)
        {
            if (!squares[new KeyValuePair<int, int>(i,index.Value)].isFilled)
            {
                isFilledX=false;
            }
        }
        for (int i = 0; i < levelModel.yStickCount; i++)
        {
            if (!squares[new KeyValuePair<int, int>(index.Key,i)].isFilled)
            {
                isFilledY=false;
            }
        }

        return isFilledX || isFilledY;
    }

    public bool isStickGroupCloseToAnySlotGroup(StickGroupController stickGroupController, GridStickState gridStickState)
    {
        for (int i = 0; i < nearestGridStickControllers.Count; i++)
        {
            if (nearestGridStickControllers[i].isFilling)
            {
                nearestGridStickControllers[i].ChangeState(GridStickState.Empty);
            }
        }
        nearestGridStickControllers.Clear();
        List<StickModel> stickModels = stickGroupController.stickGroup.sticks;
        List<HandStickController> stickControllers = stickGroupController.stickControllers;
        Debug.Log(stickModels.Count+"-"+stickControllers.Count);

        List<KeyValuePair<KeyValuePair<int, int>, GridStickController>> nearestList = sticks[stickModels[0].direction].Where(item => 
            Vector2.Distance(stickControllers[0].comparisonPosition, item.Value.transform.position) < stickHeight).ToList();
        if (nearestList.Count != 0)
            nearestList = nearestList.OrderBy(item => Vector2.Distance(stickControllers[0].comparisonPosition, item.Value.transform.position)).ToList();
        else return false;

        KeyValuePair<KeyValuePair<int, int>, GridStickController> nearest = new ();
        if (nearestList.Count != 0)
            nearest = nearestList.First();
        else return false;
        

        KeyValuePair<int, int> originGridPoint=new KeyValuePair<int, int>(nearest.Key.Key-stickModels[0].x,nearest.Key.Value-stickModels[0].y);
        for (int i = 0; i < stickModels.Count; i++)
        {
            KeyValuePair<int, int> pos = new KeyValuePair<int, int>(stickModels[i].x + originGridPoint.Key, stickModels[i].y + originGridPoint.Value);
            if (!sticks[stickModels[i].direction].ContainsKey(pos) || sticks[stickModels[i].direction][pos].isFilled)return false;

            nearestGridStickControllers.Add(sticks[stickModels[i].direction][pos]);
        }

        for (int i = 0; i < nearestGridStickControllers.Count; i++)
        {
            nearestGridStickControllers[i].ChangeState(gridStickState);
        }
        if (gridStickState == GridStickState.Filled)
            StartCoroutine(OnStickPlaced());
        return true;
    }
    
    public bool isStickGroupPlacable(StickGroupController stickGroupController)
    {
        if (stickGroupController == null  || stickGroupController.stickGroup==null) return false;

        List<StickModel> stickModels = stickGroupController.stickGroup.sticks;
        List<HandStickController> stickControllers = stickGroupController.stickControllers;
        Debug.Log(stickModels.Count+"-"+stickControllers.Count);

        Dictionary<KeyValuePair<int, int>, GridStickController> gridStickControllers =sticks[stickModels[0].direction];
        bool isStickGroupStickGroupPlacable = false;
        bool isThisSlotPlacable = true;

        for (int i = 0; i < gridStickControllers.Count; i++)
        {
            isThisSlotPlacable = true;
            KeyValuePair<int, int> originGridPoint=new KeyValuePair<int, int>(gridStickControllers.ElementAt(i).Key.Key-stickModels[0].x,gridStickControllers.ElementAt(i).Key.Value-stickModels[0].y);
            for (int j = 0; j < stickModels.Count; j++)
            {
                KeyValuePair<int, int> pos = new KeyValuePair<int, int>(stickModels[j].x + originGridPoint.Key, stickModels[j].y + originGridPoint.Value);
                if (!sticks[stickModels[j].direction].ContainsKey(pos) ||
                    sticks[stickModels[j].direction][pos].isFilled)
                {
                    isThisSlotPlacable = false;
                    break;
                }

                nearestGridStickControllers.Add(sticks[stickModels[j].direction][pos]);
            }

            if (isThisSlotPlacable) return true;
        }
        return false;
    }

    public IEnumerator OnStickPlaced()
    {
        isSquareMade = false;
        yield return new WaitForSeconds(0.5f);
        if (isSquareMade)
            ReactionController.instance.OnSquareMade();
        else
            ReactionController.instance.OnSquareCouldntMade();
    }
    private void OnDestroy()
    {
        for (int i = 0; i < squares.Count; i++)
        {
            squares.ElementAt(i).Value.OnSquareValueChanged -= OnSquareValueChanged;
        }
        for (int i = 0; i < squares.Count(); i++)
        {
            PoolingManager.instance.ReturnObj(BundleKeys.GridSquareController,squares.ElementAt(i).Value.gameObject);
        }
        for (int i = 0; i < sticks.Count(); i++)
        {
            for (int j = 0; j < sticks.ElementAt(i).Value.Count; j++)
            {
                PoolingManager.instance.ReturnObj(BundleKeys.GridStickController,sticks.ElementAt(i).Value.ElementAt(j).Value.gameObject);
            }
        }
        for (int i = 0; i < cornerSquares.Count(); i++)
        {
            PoolingManager.instance.ReturnObj(BundleKeys.GridCornerSquareController,cornerSquares.ElementAt(i).Value.gameObject);
        }
    }
}