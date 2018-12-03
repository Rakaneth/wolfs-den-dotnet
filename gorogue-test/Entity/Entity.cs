using System;
using GoRogue;
using SadConsole;
using GoRogueTest.Map;

namespace GoRogueTest.Entity
{
  public class GameEntity
  {
    public string ID {get;}
    public string Name {get; set;}
    private Coord _pos;
    public Coord Position {
      get => _pos;
      set
      {
        _pos = value;
        if (DrawEntity != null)
          DrawEntity.Position = new Microsoft.Xna.Framework.Point(_pos.X, _pos.Y);
      }
    }
    public SadConsole.Entities.Entity DrawEntity {get; set;}
    public string MapID {get; set;}
    public TileMap Map => World.Instance.GetMap(MapID);

    public GameEntity(string id)
    {
      ID = id;
    }

    public GameEntity() : this(System.Guid.NewGuid().ToString()) {}
  }
}