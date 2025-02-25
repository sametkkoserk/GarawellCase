using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameModels
{
  public enum TargetType
  {
    Point,
    Diamond
  }
  [Serializable]
  public class GameDefaultSettings
  {
    public Color emptyStickColor;
  }
  [Serializable]
  public class LevelModel
  {
    public int xStickCount;
    public int yStickCount;
    public Color color;
    public List<TargetModel> targets = new List<TargetModel>();
    public int prize;
  }

  [Serializable]
  public class TargetModel
  {
    public TargetType targetType;
    public int targetAmount;    
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