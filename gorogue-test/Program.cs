#define MAIN

using System;
using SadConsole;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using GoRogueTest.UI;


namespace GoRogueTest
{
  class Program
  {
    private const int width = 100;
    private const int height = 40;
    public static void Main(string[] args)
    {
      SadConsole.Game.Create("IBM.font", width, height);
      SadConsole.Game.OnInitialize = Init;
      SadConsole.Game.OnUpdate = Update;
      World.Create(0xDEADBEEF);
      SadConsole.Game.Instance.Run();

      //after game closes
      SadConsole.Game.Instance.Dispose();
    }

    public static void Init()
    {
      UIManager.register(
        new TitleScreen(),
        new CharGenScreen(),
        new PlayScreen()
      );
      UIManager.setScreen("title");
    }

    public static void Update(GameTime t)
    {
      UIManager.CurrentScreen.HandleKeys();
    }
  }
}
