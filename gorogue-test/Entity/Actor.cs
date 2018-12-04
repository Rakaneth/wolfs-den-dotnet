using GoRogue;
using System.Collections.Generic;
using System.Linq;

namespace GoRogueTest.Entity
{
  public class Actor: GameEntity
  {
    public bool IsPlayer{get;}
    public Actor(string id, bool isPlayer = false): base(id) 
    {
      IsPlayer = isPlayer;
    }
    public Actor(): base() {}
    private FOV _fov;
    public IEnumerable<Coord> Visible => _fov.CurrentFOV;
    public void MoveBy(int dx, int dy) => Position = Position.Translate(dx, dy);
    public void SetFOV() => _fov = new FOV(Map.ResConverter);

    public void UpdateFOV()
    {
      _fov.Calculate(Position, 6.0);
      if (IsPlayer)
        foreach(var pos in Visible)
          Map.Explore(pos);
    }
        

    public bool CanSee(Coord c) => Visible.FirstOrDefault(cand => cand == c) != null;   
    public bool CanSee(GameEntity thing) => CanSee(thing.Position);
  }

}