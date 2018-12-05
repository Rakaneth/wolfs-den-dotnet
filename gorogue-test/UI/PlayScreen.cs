using SadConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using GoRogueTest.Map;
using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using GoRogueTest.Entity;

namespace GoRogueTest.UI
{
  public class PlayScreen: Screen
  {
    #region ConsoleDimensions

    private const int MAPFULLW = 100;
    private const int MAPFULLH = 100;
    private const int MAPW = 50;
    private const int MAPH = 30;
    private const int MAPX = 0;
    private const int MAPY = 0;
    private const int STATW = 48;
    private const int STATH = 28;
    private const int STATX = 51;
    private const int STATY = 1;
    private const int MSGW = 48;
    private const int MSGH = 8;
    private const int MSGX = 1;
    private const int MSGY = 31;
    private const int INFOW = 48;
    private const int INFOH = 8;
    private const int INFOX = 51;
    private const int INFOY = 31;
    #endregion
    #region Consoles
    private SadConsole.Console map = new SadConsole.Console(MAPFULLW, MAPFULLH)
    {
      Position = new Point(0, 0),
      ViewPort = new Microsoft.Xna.Framework.Rectangle(0, 0, MAPW, MAPH)
    };
    private ControlsConsole stats = new ControlsConsole(STATW, STATH)
    {
      Position = new Point(STATX, STATY)
    };
    private SadConsole.Console msgs = new SadConsole.Console(MSGW, MSGH)
    {
      Position = new Point(MSGX, MSGY)
    };
    private SadConsole.Console info = new SadConsole.Console(INFOW, INFOH)
    {
      Position = new Point(INFOX, INFOY),
    };
    #endregion
    private SadConsole.Entities.EntityManager em = new SadConsole.Entities.EntityManager();
    private bool gameStarted = false;
    #region Constructors
    public PlayScreen(): base("play") {}
    #endregion
    protected override void Init()
    {
      InitMap();
      InitStats();
      InitMsgs();
      InitInfo();
      StartGame();
    }
    #region InitExtensions
    private void InitMap()
    {
      Children.Add(map);
    }

    private void InitStats()
    {
      UIUtils.border(stats, "Stats");
      Children.Add(stats);
    }

    private void InitMsgs()
    {
      UIUtils.border(msgs, "Messages");
      Children.Add(msgs);
    }
    private void InitInfo()
    {
      UIUtils.border(info, "Info");
      Children.Add(info);
    }

    private void StartGame()
    {
      World.Instance.AddMap("mine", MapGenerator.Instance.Caves(85, 85, false));
      var anim = new SadConsole.Surfaces.Animated("cursor", 1, 1);
      var frame = anim.CreateFrame();
      frame[0].Glyph = 'X';
      frame[0].Foreground = Color.Yellow;
      frame[0].Background = Color.Transparent;
      var cursorE = new SadConsole.Entities.Entity(anim);
      var cursor = new ActorBuilder("Cursor")
        .WithEntity(cursorE)
        .WithPosition(0, 0)
        .WithName("Test")
        .WithStartMap("mine")
        .MakePlayer()
        .Build();
      
      World.Instance.AddEntities(cursor);
      ChangeMap("mine");
      map.Children.Add(em);
      UpdateMap();
      gameStarted = true;
    }
    #endregion

    #region KeyHandler
    public override void HandleKeys()
    {
      //TODO: Better pattern
      var keys = SadConsole.Global.KeyboardState;
      var cursor = World.Instance.GetByID<Actor>("Cursor");
      if (keys.IsKeyPressed(Keys.Left))
      {
        cursor.MoveBy(-1, 0);
      }
      else if (keys.IsKeyPressed(Keys.Right))
      {
        cursor.MoveBy(1, 0);
      }
      else if (keys.IsKeyPressed(Keys.Up))
      {
        cursor.MoveBy(0, -1);
      }
      else if (keys.IsKeyPressed(Keys.Down))
      {
        cursor.MoveBy(0, 1);
      }
      else
      {
        //do nothing
      }

      if (cursor.Moved)
      {
        cursor.UpdateFOV();
        UpdateMap();
        map.CenterViewPortOnPoint(cursor.DrawEntity.Position);
        cursor.Update();
      }
    }
    #endregion

    private void UpdateMap()
    {
      var cursor = World.Instance.GetByID<Actor>("Cursor");
      map.Clear();
      TileInfo info;
      foreach (var pos in World.Instance.CurMap.Positions)
      {
        info = World.Instance.CurMap.GetInfo(pos);
        Cell curCell = null;
        
        if (World.Instance.CurMap.Light || cursor.CanSee(pos))
          curCell = new Cell(Color.White, Color.Black, info.Glyph);
        else if (World.Instance.CurMap.IsExplored(pos))
          curCell = new Cell(Color.Gray, Color.Black, info.Glyph);
        
        if (curCell != null)
          map.Cells[pos.ToIndex(map.Width)] = curCell;
      }
    }

    private void ChangeMap(string mapID)
    {
      World.Instance.CurMapID = mapID;
      em.Entities.RemoveAll();
      foreach(var thing in World.Instance.CurThings)
        em.Entities.Add(thing.DrawEntity);
    }
  }
}