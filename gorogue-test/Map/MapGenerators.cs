using GoRogue;
using GoRogue.Random;
using GoRogue.MapGeneration.Connectors;
using System.Collections.Generic;
using GoRogue.MapGeneration;
using Troschuetz.Random;
using System;

namespace GoRogueTest.Map
{
  public class MapGenerators
  {
    private const int MAX_ROOMS = 20;
    private const int MIN_SIZE = 7;
    private const int MAX_SIZE = 20;
    private const int MAX_ATTEMPTS = 10;
    private const int MIN_ROOM = 5;
    static private IDictionary<BSPTree, Rectangle> bspDict = new Dictionary<BSPTree, Rectangle>();
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

    private class BSPTree
    {
      public readonly Rectangle _rect;
      private BSPTree _left = null;
      private BSPTree _right = null;
      private IGenerator _rng;
      public BSPTree Left => _left;
      public BSPTree Right => _right;
      public bool IsLeaf => _left == null && _right == null;
      public BSPTree(int x, int y, int w, int h, IGenerator rng = null)
      {
        _rect = new Rectangle(x, y, w, h);
        if (rng == null)
        {
          _rng = SingletonRandom.DefaultRNG;
        }
        else
        {
          _rng = rng;
        }
      }

      private bool canSplitHorz()
      {
        float w = (float)_rect.Width;
        float h = (float)_rect.Height;
        if (h/w >= 1.25f)
          return true;
        else if (w/h >= 1.25f)
          return false;
        else
          return _rng.NextBoolean();
      }

      private bool split()
      {
        if (!IsLeaf)
          return false;
        bool horz = canSplitHorz();
        int max = (horz ? _rect.Height : _rect.Width) - MIN_SIZE;
        if (max < MIN_SIZE)
          return false;
        int roll = _rng.Next(max);
        roll = Math.Max(MIN_SIZE, roll);
        if (horz)
        {
          _left = new BSPTree(_rect.X, _rect.Y, _rect.Width, roll);
          _right = new BSPTree(_rect.X, _rect.Y+roll, _rect.Width, _rect.Height-roll);
        }
        else
        {
          _left = new BSPTree(_rect.X, _rect.Y, roll, _rect.Height);
          _right = new BSPTree(_rect.X+roll, _rect.Y, _rect.Width-roll, _rect.Height);
        }
        return true;
      }

      public void Build()
      {
        bool mustSplit = _rect.Width > MAX_SIZE || _rect.Height > MAX_SIZE;
        bool mightSplit = _rng.NextBoolean();
        if ((mustSplit || mightSplit) && split())
        {
          _left.Build();
          _right.Build();
        }
        /* 
        else
        {

        }
        */
      }

      public BSPTree GetLeaf()
      {
        if (IsLeaf)
          return this;
        else if (_rng.NextBoolean())
          return _left.GetLeaf();
        else
          return _right.GetLeaf();
      }
    }

    static public TileMap BSP(int width, int height)
    {
      var baseMap = new TileMap(width, height);
      var bspTree = new BSPTree(0, 0, width, height);
      bspTree.Build();
      bspRooms(bspTree, baseMap);
      baseMap.GetRooms();
      bspCorridors(bspTree, baseMap);
      return baseMap;
    }

    static private void bspRooms(BSPTree root, TileMap map)
    {
      var rng = SingletonRandom.DefaultRNG;
      if (root.IsLeaf)
      {          
        int w = rng.Next(MIN_ROOM, root._rect.Width - 2);
        int h = rng.Next(MIN_ROOM, root._rect.Height - 2);
        int dx = rng.Next(1, root._rect.Width - w - 1);
        int dy = rng.Next(1, root._rect.Height - h - 1);
        var rect = new Rectangle(dx + root._rect.X, dy + root._rect.Y, w, h);
        carve(rect, map);
        bspDict.Add(root, rect);
      }
      else
      {
        bspRooms(root.Left, map);
        bspRooms(root.Right, map);
      }
    }

    static private void bspCorridors(BSPTree root, TileMap map)
    {
      var stack = new Stack<BSPTree>();
      var q = new Queue<BSPTree>();
      q.Enqueue(root);
      while (q.Count > 0)
      {
        var next = q.Dequeue();
        stack.Push(next);
        if (!next.IsLeaf)
        {
          q.Enqueue(next.Left);
          q.Enqueue(next.Right);
        }
      }

      while(stack.Count > 0)
      {
        var sNext = stack.Pop();
        if (sNext != root)
        {
          var sConnect = stack.Pop();
          Connect(map, sNext, sConnect);
        }
      }
    }

    static private void Connect(TileMap map, BSPTree hither, BSPTree yon)
    {
      var rng = SingletonRandom.DefaultRNG;
      var hitherLeaf = hither.GetLeaf();
      var yonLeaf = yon.GetLeaf();
      var hitherRect = bspDict[hitherLeaf];
      var yonRect = bspDict[yonLeaf];
      int xDist = Math.Abs(hitherRect.X - yonRect.X);
      int yDist = Math.Abs(hitherRect.Y - yonRect.Y);
      Coord hitherPt, yonPt;
      
      if (yDist >= xDist || rng.NextBoolean())
        (hitherPt, yonPt) = getStartPoints(hitherRect, yonRect, false);
      else
        (hitherPt, yonPt) = getStartPoints(hitherRect, yonRect, true);
      

      if (rng.NextBoolean())
      {
        lineConnect(map, hitherPt.X, yonPt.X, yonPt.Y, true);
        lineConnect(map, hitherPt.Y, yonPt.Y, yonPt.X, false);
      }
      else
      {
        lineConnect(map, hitherPt.Y, yonPt.Y, yonPt.X, false);
        lineConnect(map, hitherPt.X, yonPt.X, yonPt.Y, true);
      }
    }

    static private Tuple<Coord, Coord> getStartPoints(Rectangle hither, Rectangle yon, bool horz)
    {
      Rectangle from = hither, to = yon;
      Coord fromPt, toPt;
      if (horz)
      {
        //rects are being connected horizontally
        if (hither.X > yon.X)
        {
          to = hither;
          from = yon;
        }
        fromPt = from.RandomPosition(c => c.X == from.MaxExtentX);
        toPt = to.RandomPosition(c => c.X == to.X);
      }
      else
      {
        //rects are being connected vertically
        if (hither.Y > to.Y)
        {
          to = hither;
          from = yon;
        }
        fromPt = from.RandomPosition(c => c.Y == from.MaxExtentY);
        toPt = to.RandomPosition(c => c.Y == to.Y);
      }

      return new Tuple<Coord, Coord>(fromPt, toPt);
    }

    static private void lineConnect(TileMap map, int start, int end, int at, bool horz)
    {
      int min = Math.Min(start, end);
      int max = Math.Max(start, end);
      for (int i=min; i<=max; i++)
      {
        if (horz)
        {
          map[i, at] = Tile.FLOOR;
        }
        else
        {
          map[at, i] = Tile.FLOOR;
        }
      }
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
      for (int x=room.X; x<=room.MaxExtentX; x++)
      {
        for (int y=room.Y; y<=room.MaxExtentY; y++)
        {
          map[x, y] = Tile.FLOOR;
        }
      }
    }
  }
}