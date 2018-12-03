//global world state
using Troschuetz.Random.Generators;
using Troschuetz.Random;
using System;
using System.Collections.Generic;
using GoRogueTest.Map;
using GoRogueTest.Entity;

namespace GoRogueTest
{
  public class World
  {
    static private World _instance;
    private IGenerator _rng;
    static public World Instance
    {
      get
      {
        if (_instance == null)
          throw new Exception("World must be created with World.Create() first");
        return _instance;
      }
    }

    public IGenerator RNG => _rng;
    private IDictionary<string, TileMap> _maps = new Dictionary<string, TileMap>();
    public string CurMapID;
    public TileMap CurMap => GetMap(CurMapID);
    private IDictionary<string, GameEntity> _things = new Dictionary<string, GameEntity>();



    static public void Create(uint seed) =>_instance = new World(seed);
    static public void Create() => _instance = new World();

    private World(uint seed) 
    {
      _rng = new XorShift128Generator(seed);
    }

    private World()
    {
      _rng = new XorShift128Generator();
    }

    public TileMap GetMap(string mapID) => _maps[mapID];
    public T GetByID<T>(string eID) where T: GameEntity => _things[eID] as T;
    public void AddMap(string mapID, TileMap map) => _maps[mapID] = map;
    
    
  }
}