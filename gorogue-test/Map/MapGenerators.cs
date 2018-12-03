using GoRogue;
using GoRogue.Random;
using GoRogue.MapGeneration.Connectors;
using System.Collections.Generic;
using GoRogue.MapGeneration;
using Troschuetz.Random;
using System;
using GoRogue.MapViews;

namespace GoRogueTest.Map
{
  public class MapGenerator
  {
    private static MapGenerator _instance;
    private IGenerator _rng = SingletonRandom.DefaultRNG;
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

    private void carve(ISettableMapView<Tile> map, Rectangle rect)
    {
      
    }


  }
}