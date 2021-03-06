//global world state
using Troschuetz.Random.Generators;
using Troschuetz.Random;
using System;
using System.Collections.Generic;
using GoRogueTest.Map;
using GoRogueTest.Entity;
using System.Linq;

namespace GoRogueTest
{
  public class World
  {
    static private World _instance;
    private IGenerator _rng;
    public GameConfigs Configs {get; private set;}
    public Actor Player {get; private set;}
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
    public IEnumerable<GameEntity> CurThings
    {
      get =>_things.Values.Where(thing => thing.MapID == CurMapID);
    }
    
    static public void Create(uint seed, GameConfigs configs)
    {
      _instance = new World(seed, configs);
    }

    static public void Create(GameConfigs configs) => _instance = new World(configs);
    static public void Create(uint seed) =>_instance = new World(seed);
    static public void Create() => _instance = new World();

    private World(uint seed, GameConfigs configs) 
    {
      _rng = new XorShift128Generator(seed);
      Configs = configs;
    }

    private World(uint seed): this(seed, new GameConfigs(logging: true)) {}

    private World(GameConfigs configs)
    {
      _rng = new XorShift128Generator();
      Configs = configs;
    }

    private World(): this(new GameConfigs(logging: true)) {}

    public TileMap GetMap(string mapID) => _maps[mapID];
    public T GetByID<T>(string eID) where T: GameEntity => _things[eID] as T;
    public void AddMap(string mapID, TileMap map) => _maps[mapID] = map;
    public void AddMap(TileMap map) => _maps[map.ID] = map;
    public void AddEntities(params GameEntity[] things) 
    {
      foreach (var thing in things)
        _things[thing.ID] = thing;
    }

    public void SetPlayer(Actor newPlayer)
    {
      Player = newPlayer;
      AddEntities(Player);
    }

    public void GenerateAllMaps()
    {
      //TODO: finish
    }
  }
}