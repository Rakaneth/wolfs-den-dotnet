using GoRogue.MapViews;

namespace GoRogueTest.Map
{
  public class TileMap: ArrayMap<Tile>
  {
    public TileMap(int width, int height): base(width, height) {}
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