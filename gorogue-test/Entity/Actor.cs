using GoRogue;
using System.Collections.Generic;
using System.Linq;

namespace GoRogueTest.Entity
{
  public class Actor: GameEntity
  {
    public Actor(string id): base(id) {}
    public Actor():  base() {}
    private FOV _fov;
    public IEnumerable<Coord> Visible => _fov.CurrentFOV;
    public void MoveBy(int dx, int dy) => Position = Position.Translate(dx, dy);
    public void SetFOV() => _fov = new FOV(Map.ResConverter);

    public void UpdateFOV() => _fov.Calculate(Position, 6.0);

    public bool CanSee(Coord c) => Visible.First(cand => cand == c) != null;   
    public bool CanSee(GameEntity thing) => CanSee(thing.Position);
  }

}