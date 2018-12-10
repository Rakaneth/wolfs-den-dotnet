using Microsoft.Xna.Framework;
using SadConsole;
using System.Collections.Generic;

namespace GoRogueTest.Map
{
  public enum Tile
  {
    WALL,
    FLOOR,
    CLOSEDOOR,
    OPENDOOR,
    UPSTAIRS,
    DOWNSTAIRS,
    DEEPWATER,
    SHALLOWWATER,
    INDOOR,
    OUTDOOR,
    NULL
  }

  public struct TileInfo
  {
    public int Glyph {get;}
    public bool Walk {get;}
    public bool See {get;}

    public TileInfo(int glyph, bool walk = true, bool see = true)
    {
      Glyph = glyph;
      Walk = walk;
      See = see;
    }
  }

  public static class TileData
  {
    private static IDictionary<Tile, TileInfo> data = new Dictionary<Tile, TileInfo>()
    {
      [Tile.WALL] = new TileInfo('#', false, false),
      [Tile.FLOOR] = new TileInfo(0xF9),
      [Tile.CLOSEDOOR] = new TileInfo('+', false, false),
      [Tile.OPENDOOR] = new TileInfo(0xB3),
      [Tile.UPSTAIRS] = new TileInfo('<'),
      [Tile.DOWNSTAIRS] = new TileInfo('>'),
      [Tile.DEEPWATER] = new TileInfo('~', false, false),
      [Tile.SHALLOWWATER] = new TileInfo('~'),
      [Tile.NULL] = new TileInfo(0, false, false),
      [Tile.INDOOR] = new TileInfo(0xEF, true, true),
      [Tile.OUTDOOR] = new TileInfo(0xEF, true, true)
    };

    public static TileInfo GetInfo(Tile tile) => data[tile];
  }

}
