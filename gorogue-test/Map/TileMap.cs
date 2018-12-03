using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using System.Collections.Generic;

namespace GoRogueTest.Map
{
  public class TileMap
  {
    private ArrayMap<Tile> _tiles;
    public List<IReadOnlyMapArea> _rooms;
    public readonly SetTileMapTranslator SetConverter;
    public readonly WalkableTileMapTranslator WalkConverter;
    public readonly VisibleTileMapTranslator SeeConverter;
    public readonly LightResTileMapDoubleTranslator ResConverter;
    public int Width => _tiles.Width;
    public int Height => _tiles.Height;
    public IEnumerable<Coord> Positions => _tiles.Positions();
    public ISettableMapView<Tile> Tiles => _tiles;
    public Tile this[int x, int y]
    {
      get => _tiles[x, y];
      set => _tiles[x, y] = value;
    }
    public Tile this[Coord c]
    {
      get => this[c.X, c.Y];
      set => this[c.X, c.Y] = value;
    }

    public TileMap(int width, int height)
    {
      _tiles = new ArrayMap<Tile>(width, height);
      SetConverter = new SetTileMapTranslator(_tiles);
      WalkConverter = new WalkableTileMapTranslator(_tiles);
      SeeConverter = new VisibleTileMapTranslator(_tiles);
      ResConverter = new LightResTileMapDoubleTranslator(_tiles);
    }

    public bool InBounds(int x, int y) => _tiles.Bounds().Contains(x, y);
    public bool InBounds(Coord c) => InBounds(c.X, c.Y);
    public void AllTile(Tile tile)
    {
      foreach (var pos in this.Positions)
      {
        this[pos] = tile;
      }
    }

    public TileInfo GetInfo(int x, int y)
    {
      var tile = InBounds(x, y) ? this[x, y] : Tile.NULL;
      return TileData.GetInfo(tile);
    }

    public TileInfo GetInfo(Coord c) => GetInfo(c.X, c.Y);

    public void GetRooms()
    {
      var finder = new MapAreaFinder(SeeConverter, AdjacencyRule.CARDINALS);
      _rooms = new List<IReadOnlyMapArea>(finder.MapAreas());
    }
  }
  public class VisibleTileMapTranslator: TranslationMap<Tile, bool>
  {
    public VisibleTileMapTranslator(IMapView<Tile> baseMap): base(baseMap) {}

    protected override bool TranslateGet(Tile value)
    {
      var data = TileData.GetInfo(value);
      return data.See;
    }
  }

  public class LightResTileMapDoubleTranslator: TranslationMap<Tile, double>
  {
    public LightResTileMapDoubleTranslator(IMapView<Tile> baseMap): base(baseMap){}
    protected override double TranslateGet(Tile value)
    {
      var data = TileData.GetInfo(value);
      return data.See ? 0.0 : 1.0;
    }
  }

  public class WalkableTileMapTranslator: TranslationMap<Tile, bool>
  {
    public WalkableTileMapTranslator(IMapView<Tile> baseMap): base(baseMap) {}

    protected override bool TranslateGet(Tile value)
    {
      var data = TileData.GetInfo(value);
      return data.Walk;
    }
  }

  public class SetTileMapTranslator: SettableTranslationMap<Tile, bool>
  {
    public SetTileMapTranslator(ISettableMapView<Tile> baseMap): base(baseMap) {}

    protected override bool TranslateGet(Tile value)
    {
      var data = TileData.GetInfo(value);
      return data.Walk;
    }

    protected override Tile TranslateSet(bool value)
    {
      return value ? Tile.FLOOR : Tile.WALL;
    }
  }

  
}