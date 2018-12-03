using GoRogue;
using GoRogue.Random;
using GoRogue.MapGeneration.Connectors;
using System.Collections.Generic;
using GoRogue.MapGeneration;
using Troschuetz.Random;
using System;
using GoRogue.MapViews;
using BoneGen;

namespace GoRogueTest.Map
{
  public class MapGenerator
  {
    private static MapGenerator _instance;
    private readonly BoneGen.BoneGen bg = new BoneGen.BoneGen();
    public static MapGenerator Instance
    {
      get 
      {
        if (_instance == null)
          _instance = new MapGenerator();
        return _instance;
      }
    }

    private MapGenerator() {}

    public TileMap Uniform(int width, int height, bool isLight=true)
    {
      var baseMap = BoneGen.BoneGen.WallWrap(
        bg.Generate(TilesetType.ROOMS_LIMIT_CONNECTIVITY, height, width));
      return TileMap.ToTileMap(baseMap, isLight);
    }
  }
}