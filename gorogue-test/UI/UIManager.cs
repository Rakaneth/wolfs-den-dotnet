using SadConsole;
using System.Collections.Generic;
using System;

namespace GoRogueTest.UI
{
  public class UIManager
  {
    private static Dictionary<string, Screen> screens = new Dictionary<string, Screen>();
    public static Screen CurrentScreen
    {
      get 
      {
        return SadConsole.Global.CurrentScreen as Screen;
      }
    }

    public static void setScreen(string screenName)
    {
      Screen prevScreen;
      prevScreen = CurrentScreen;
      
      var screen = screens[screenName];
      if (prevScreen != null) 
      {
        prevScreen.exit();
        prevScreen.IsVisible = false;
        prevScreen.IsFocused = false;
      }
      screen.IsVisible = true;
      screen.IsFocused = true;
      SadConsole.Global.CurrentScreen = screen;
      screen.enter();
    }
    public static void register(params Screen[] screenies)
    {
      foreach (var screen in screenies)
      {
        screens[screen.Name] = screen;
      }
    }
  }
}