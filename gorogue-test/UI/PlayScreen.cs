using SadConsole;
using Microsoft.Xna.Framework;

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
    private Console map = new Console(MAPFULLW, MAPFULLH)
    {
      Position = new Point(0, 0),
      ViewPort = new Rectangle(0, 0, MAPW, MAPH)
    };
    private ControlsConsole stats = new ControlsConsole(STATW, STATH)
    {
      Position = new Point(STATX, STATY)
    };
    private Console msgs = new Console(MSGW, MSGH)
    {
      Position = new Point(MSGX, MSGY)
    };
    private Console info = new Console(INFOW, INFOH)
    {
      Position = new Point(INFOX, INFOY),
    };
    #endregion
    #region Constructors
    public PlayScreen(): base("play") {}
    #endregion
    protected override void Init()
    {
      InitMap();
      InitStats();
      InitMsgs();
      InitInfo();
    }
    #region InitExtensions
    private void InitMap()
    {
      map.FillWithRandomGarbage();
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
    #endregion
    #region KeyHandler
    public override void HandleKeys()
    {
      //TODO: implementation
    }
    #endregion
  }
}