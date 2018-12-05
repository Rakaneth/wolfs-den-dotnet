using SadConsole;
using System;
using System.Collections.Generic;

namespace GoRogueTest.UI
{
  public abstract class Screen: ConsoleContainer
  {
    public string Name {get; }
    public Screen(string name)
    {
      IsVisible = false;
      IsFocused = false;
      Name = name;
      Init();
    }

    abstract protected void Init();
    abstract public void HandleKeys();

    virtual public void enter()
    {
      System.Console.WriteLine($"Entered {Name} screen.");
    }

    virtual public void exit()
    {
      System.Console.WriteLine($"Exited {Name} screen.");
    }
  }
}