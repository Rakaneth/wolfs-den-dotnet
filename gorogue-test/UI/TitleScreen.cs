using SadConsole;

namespace GoRogueTest.UI
{
  public class TitleScreen: Screen
  {
    private Console title = new Console(100, 40);
    
    public TitleScreen(): base("title") {}

    protected override void Init()
    {
      title.Print(0, 0, "Screens are working");
      this.Children.Add(title);
    }

    public override void HandleKeys()
    {
      if (SadConsole.Global.KeyboardState.KeysPressed.Count > 0)
      {
        UIManager.setScreen("chargen");
      }
    }
  }
}