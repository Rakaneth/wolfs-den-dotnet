using SadConsole;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using GoRogueTest.Map;
using GoRogue;
using GoRogue.MapGeneration;
using GoRogue.MapViews;

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

    private SadConsole.Entities.Entity cursor;
    private SadConsole.Entities.EntityManager em = new SadConsole.Entities.EntityManager();
    private ArrayMap<Tile> tiles; //TODO: Game state
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
      InitCursor();
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

    private void InitCursor()
    {
      var anim = new SadConsole.Surfaces.Animated("cursor", 1, 1);
      var frame = anim.CreateFrame();
      frame[0].Glyph = 'X';
      frame[0].Foreground = Color.Yellow;
      frame[0].Background = Color.Transparent;
      cursor = new SadConsole.Entities.Entity(anim);
      cursor.Position = new Point(0, 0);
      em.Entities.Add(cursor);
      map.Children.Add(em);
    }
    #endregion

    #region Enter-Exit
    public override void enter()
    {
      if (!gameStarted)
      {
        tiles = new ArrayMap<Tile>(50, 50);
        var convert = new SetTileMapTranslator(tiles);
        GoRogue.MapGeneration.Generators.RectangleMapGenerator.Generate(convert);
        gameStarted = true;
        UpdateMap();
      }
      base.enter();
    }
    #endregion
    #region KeyHandler
    public override void HandleKeys()
    {
      //TODO: Better pattern
      var keys = SadConsole.Global.KeyboardState;
      if (keys.IsKeyPressed(Keys.Left))
      {
        cursor.Position += new Point(-1, 0);
      }
      else if (keys.IsKeyPressed(Keys.Right))
      {
        cursor.Position += new Point(1, 0);
      }
      else if (keys.IsKeyPressed(Keys.Up))
      {
        cursor.Position += new Point(0, -1);
      }
      else if (keys.IsKeyPressed(Keys.Down))
      {
        cursor.Position += new Point(0, 1);
      }
      else
      {
        //do nothing
      }
      map.CenterViewPortOnPoint(cursor.Position);
    }
    #endregion

    private void UpdateMap()
    {
      foreach (var pos in tiles.Positions())
      {
        var info = TileData.GetInfo(tiles[pos.X, pos.Y]);
        map.Cells[pos.ToIndex(map.Width)] = new Cell(Color.White, Color.Black, info.Glyph);
      }
    }
  }
}