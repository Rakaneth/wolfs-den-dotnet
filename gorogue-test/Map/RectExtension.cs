using GoRogue;
using System.Linq;
using System.Collections.Generic;

namespace GoRogueTest.Map
{
  public static class RectExtension
  {
    public static IEnumerable<Coord> Border(this Rectangle rect)
    {
      return rect
        .Positions()
        .Where(c => c.X == rect.X || c.X == rect.MaxExtentX || c.Y == rect.Y || c.Y == rect.MaxExtentY);
    }

    public static bool OnBorder(this Rectangle rect, int x, int y)
    {
      return rect.Border().Contains(Coord.Get(x, y));
    }

    public static bool OnBorder(this Rectangle rect, Coord c)
    {
      return rect.Border().Contains(c);
    }
  }
} 