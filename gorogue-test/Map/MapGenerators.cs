using GoRogue;
using GoRogue.Random;
using GoRogue.MapGeneration.Connectors;
using System.Collections.Generic;
using GoRogue.MapGeneration;

namespace GoRogueTest.Map
{
  public static class MapGenerators
  {
    private const int MAX_ROOMS = 20;
    private const int MIN_SIZE = 5;
    private const int MAX_SIZE = 12;
    private const int MAX_ATTEMPTS = 10;
    static public TileMap Uniform(int width, int height, bool makeDoors = true)
    {
      var baseMap = new TileMap(width, height);
      var rng = SingletonRandom.DefaultRNG;
      baseMap.AllTile(Tile.WALL);
      var rooms = new List<Rectangle>();
      int tries = 0;
      var tunneller = new HorizontalVerticalTunnelCreator();
      var finder = new MapAreaFinder(baseMap.SeeConverter, AdjacencyRule.CARDINALS);
      for (int r=0; r<MAX_ROOMS; r++)
      {
        int w = rng.Next(MIN_SIZE, MAX_SIZE+1);
        int h = rng.Next(MIN_SIZE, MAX_SIZE+1);
        int x = rng.Next(baseMap.Width - w + 1);
        int y = rng.Next(baseMap.Height - h + 1);
        var newRoom = new Rectangle(x, y, w, h);
        bool intersects = roomIntersect(newRoom, rooms);
        while (intersects && tries < MAX_ATTEMPTS)
        {
          x = rng.Next(baseMap.Width - w + 1);
          y = rng.Next(baseMap.Height - h + 1);
          newRoom = new Rectangle(x, y, w, h);
          intersects = roomIntersect(newRoom, rooms);
          tries++;
        }
        if (!intersects)
        {
          rooms.Add(newRoom);
          carve(newRoom, baseMap);
          if (rooms.Count > 1)
          {
            tunneller.CreateTunnel(
              baseMap.SetConverter, 
              newRoom.Center, 
              rooms[rooms.Count-2].Center);
          }
        }
      }
      return baseMap;
    }

    static private bool roomIntersect(Rectangle room, List<Rectangle> existing)
    {
      foreach (var rm in existing)
      {
        if (room.Intersects(rm))
        {
          return true;
        }
      }
      return false;
    }

    static private Coord getDoor(Rectangle room) 
    {
      return room.RandomPosition(c => c.X == room.X || c.Y == room.Y);
    }

    static private void carve(Rectangle room, TileMap map)
    {
      for (int x=room.X+1; x<room.MaxExtentX-1; x++)
      {
        for (int y=room.Y+1; y<room.MaxExtentY-1; y++)
        {
          map[x, y] = Tile.FLOOR;
        }
      }
    }
  }
}