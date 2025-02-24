using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameModels
{
  [Serializable]
  public class GameDefaultSettings
  {
    public Color emptyStickColor;
  }
  public class LevelModel
  {
    public int xStickCount;
    public int yStickCount;
    public Color color;
  }
  [Serializable]
  public class StickGroupModel
  {
    public List<StickModel> sticks = new List<StickModel>();
    public int minLevel;
  }

  [Serializable]
  public class StickModel
  {
    public StickModel(int _x, int _y, StickDirection _direction)
    {
      x = _x;
      y = _y;
      direction = _direction;
    }
    public int x;
    public int y;
    public StickDirection direction;
  }
}