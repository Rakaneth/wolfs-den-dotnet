using GoRogue.Random;
using GoRogue.MapGeneration.Connectors;
using Troschuetz.Random;
using System;
using GoRogue;
using GoRogue.MapGeneration;

namespace GoRogueTest.Map
{
  public class EdgeConnectionPointSelector: IAreaConnectionPointSelector
  {
    private IGenerator rng;
    public EdgeConnectionPointSelector(IGenerator rng = null)
    {
      if (rng == null)
      {
        this.rng = SingletonRandom.DefaultRNG;
      }
      else
      {
        this.rng = rng;
      }
    }

    public Tuple<Coord, Coord> SelectConnectionPoints(IReadOnlyMapArea area1, IReadOnlyMapArea area2)
    {
      bool vert = rng.NextBoolean();
      Coord ptA, ptB;
      if (vert)
      {
        IReadOnlyMapArea left = area1, right = area2;
        if (area2.Bounds.X < area1.Bounds.X)
        {
          left = area2;
          right = area1;
        }
        ptA = left.Positions.RandomItem(c => c.Y == left.Bounds.MaxExtentY);
        ptB = right.Positions.RandomItem(c => c.Y == right.Bounds.Y);
      }
      else
      {
        IReadOnlyMapArea up = area1, down = area2;
        if (area2.Bounds.Y < area1.Bounds.Y)
        {
          up = area2;
          down = area1;
        }
        ptA = up.Positions.RandomItem(c => c.X == up.Bounds.MaxExtentX);
        ptB = down.Positions.RandomItem(c => c.X == down.Bounds.X);
      }
      return new Tuple<Coord, Coord>(ptA, ptB);
    }
  }
}