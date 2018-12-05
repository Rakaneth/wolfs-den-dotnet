using System;
using GoRogue;
using SadConsole;
using GoRogueTest.Map;
using System.Collections.Generic;

namespace GoRogueTest.Entity
{
  public abstract class GameEntity
  {
    public string ID {get;}
    public string Name {get; set;}
    public string Desc{get; set;}
    private Coord _pos;
    public Coord Position {
      get => _pos;
      set
      {
        Moved = value != _pos;
        _pos = value;
        if (DrawEntity != null)
          DrawEntity.Position = new Microsoft.Xna.Framework.Point(_pos.X, _pos.Y); 
      }
    }
    public SadConsole.Entities.Entity DrawEntity {get; set;}
    public int Layer {get; set;}
    public string MapID {get; set;}
    public TileMap Map => World.Instance.GetMap(MapID);
    public bool Moved {get; private set;}
    public HashSet<string> Tags {get; private set;} = new HashSet<string>();
    public string Type {get; private set;}

    public GameEntity(string id)
    {
      ID = id;
    }

    public GameEntity() : this(System.Guid.NewGuid().ToString()) {}
    public void Update() => Moved = false;
    public bool HasTag(string tag) => Tags.Contains(tag);
    public void AddTag(string tag) => Tags.Add(tag);
    public void RemoveTag(string tag) => Tags.Remove(tag);
    public void SetType(string newType) => Type = newType;
  }
}