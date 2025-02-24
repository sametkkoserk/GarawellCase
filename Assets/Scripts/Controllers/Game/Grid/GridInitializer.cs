using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameModels;
using UnityEngine;
using UnityEngine.UI;

public enum StickDirection
{
  vertical,
  horizontal
}

[DefaultExecutionOrder(-100)]
public class GridInitializer : MonoBehaviour
{
  [SerializeField]private RectTransform gridRect;
  [SerializeField]private Transform sticksParent;
  [SerializeField]private Transform squaresParent;
  [SerializeField]private Transform cornerSquaresParent;

  private int xStickCount;
  private int yStickCount;
  
  private Dictionary<KeyValuePair<int, int>, GridSquareController> squares = new();
  private Dictionary<KeyValuePair<int, int>, GridStickController> verticalSticks = new();
  private Dictionary<KeyValuePair<int, int>, GridStickController> horizontalSticks = new();

  public float stickHeight { get; private set; }
  private LevelModel levelModel;

  private void Start()
  {
    levelModel = GameLevelManager.GetCurrentLevelModel();
    InitGrid(levelModel.xStickCount,levelModel.yStickCount);
  }

  public void InitGrid(int _xStickCount, int _yStickCount)
  {
    SetGridSize(_xStickCount,_yStickCount);
    CreateSticks();
    CreateSquares();
    CreateCornerSquares();
    
    GridManager.instance.SetGridInfo(levelModel,stickHeight,squares,verticalSticks,horizontalSticks);
  }
  
  public void SetGridSize(int _xStickCount, int _yStickCount)
  {
    xStickCount = _xStickCount;
    yStickCount = _yStickCount;
    
    float width = gridRect.rect.width-120;
    float maxHeight = GetComponent<RectTransform>().rect.height-120;
    stickHeight = Mathf.Min(width / xStickCount,maxHeight/yStickCount);
    
    gridRect.anchorMin = new Vector2(0.5f, 0);
    gridRect.anchorMax = new Vector2(0.5f, 0);

    gridRect.anchoredPosition = new Vector2(0, 60);

    gridRect.sizeDelta = new Vector2(xStickCount*stickHeight,yStickCount * stickHeight);
    Debug.Log(width+"-"+maxHeight+"-"+stickHeight);
    Debug.Log(gridRect.sizeDelta.x+"---"+gridRect.sizeDelta.y);
  }
  
  private void CreateSticks()
  {
    for (int y = 0; y < yStickCount+1; y++)
    {
      for (int x = 0; x < xStickCount+1; x++)
      {
        Vector2 pos = new Vector2(x * stickHeight, y * stickHeight);
        if (y!=yStickCount)
          CreateStick(x,y,StickDirection.vertical,pos);

        if (x!=xStickCount)
          CreateStick(x,y,StickDirection.horizontal,pos);
      }
    }
  }

  private void CreateStick(int x, int y, StickDirection stickDirection, Vector2 pos)
  {
    GameObject obj=PoolingManager.instance.GetObj(BundleKeys.GridStickController, sticksParent);
    GridStickController gridStickController = obj.GetComponent<GridStickController>();
    
    if (stickDirection==StickDirection.vertical)
      verticalSticks.Add(new KeyValuePair<int, int>(x,y),gridStickController);
    else
      horizontalSticks.Add(new KeyValuePair<int, int>(x,y),gridStickController);
    
    gridStickController.SetPosition(x,y,stickDirection,pos,stickHeight);
  }
  private void CreateSquares()
  {
    for (int y = 0; y < yStickCount; y++)
    {
      for (int x = 0; x < xStickCount; x++)
      {
        Vector2 pos = new Vector2(x * stickHeight, y * stickHeight);
        CreateSquare(x,y,pos);
      }
    }
  }

  private void CreateSquare(int x, int y, Vector2 pos)
  {
    Dictionary<SquareSide, GridStickController> stickDict = new Dictionary<SquareSide, GridStickController>(4);
    stickDict.Add(SquareSide.Left,verticalSticks[new KeyValuePair<int, int>(x, y)]);
    stickDict.Add(SquareSide.Right,verticalSticks[new KeyValuePair<int, int>(x+1, y)]);
    stickDict.Add(SquareSide.Top,horizontalSticks[new KeyValuePair<int, int>(x, y+1)]);
    stickDict.Add(SquareSide.Bottom,horizontalSticks[new KeyValuePair<int, int>(x, y)]);

    GameObject obj=PoolingManager.instance.GetObj(BundleKeys.GridSquareController, squaresParent);
    GridSquareController gridSquareController = obj.GetComponent<GridSquareController>();
    
    squares.Add(new KeyValuePair<int, int>(x,y),gridSquareController);
    gridSquareController.SetPosition(x,y,pos,stickHeight,stickDict);

  }
  
  private void CreateCornerSquares()
  {
    for (int y = 0; y < yStickCount+1; y++)
    {
      for (int x = 0; x < xStickCount+1; x++)
      {
        Vector2 pos = new Vector2(x * stickHeight, y * stickHeight);
        CreateCornerSquare(x,y,pos);
      }
    }
  }

  private void CreateCornerSquare(int x, int y, Vector2 pos)
  {
    List<GridStickController> stickList = new List<GridStickController>(4);
    if (verticalSticks.ContainsKey(new KeyValuePair<int, int>(x, y)))
      stickList.Add(verticalSticks[new KeyValuePair<int, int>(x, y)]);
    if (verticalSticks.ContainsKey(new KeyValuePair<int, int>(x, y-1)))
      stickList.Add(verticalSticks[new KeyValuePair<int, int>(x, y-1)]);
    if (horizontalSticks.ContainsKey(new KeyValuePair<int, int>(x, y)))
      stickList.Add(horizontalSticks[new KeyValuePair<int, int>(x, y)]);
    if (horizontalSticks.ContainsKey(new KeyValuePair<int, int>(x-1, y)))
      stickList.Add(horizontalSticks[new KeyValuePair<int, int>(x-1, y)]);


    GameObject obj=PoolingManager.instance.GetObj(BundleKeys.GridCornerSquareController, cornerSquaresParent);
    GridCornerSquareController gridCornerSquareController = obj.GetComponent<GridCornerSquareController>();
    
    gridCornerSquareController.SetPosition(pos,stickHeight,stickList);  
  }
}


