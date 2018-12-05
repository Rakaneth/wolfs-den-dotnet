using SadConsole;
using SadConsole.Surfaces;
using SadConsole.Controls;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace GoRogueTest.UI
{
  public class UIUtils
  {
    public static string decorate(string text, string foreground, string background="default")
    {
      return $"[c:r f:{foreground}][c:r b:{background}]{text}[c:undo][c:undo]";
    }

    public static void border(Console cons, string caption = "")
    {
      var surface = new Basic(cons.Width + 2, cons.Height + 2);
      surface.Position = new Point(-1, -1);
      surface.DrawBox(
        new Rectangle(0, 0, surface.Width, surface.Height), 
        new Cell(cons.DefaultForeground, cons.DefaultBackground), 
        null,
        SurfaceBase.ConnectedLineThick);
      surface.Print(1, 0, caption);
      cons.Children.Add(surface);
    }

    public static void AddRange(ControlsConsole control, params ControlBase[] toAdd)
    {
      foreach (var item in toAdd)
      {
        control.Add(item);
      }
    }

    public static string pluralize(string word, int val)
    {
      return val > 1 ? word + "s" : word;
    }

    public static Dictionary<string, Color> Swatch = new Dictionary<string, Color>
    {
      ["iron"] = Color.DarkBlue,
      ["crimson"] = Color.Crimson,
      ["green"] = Color.Green,
      ["cyan"] = Color.Cyan,
      ["goldenrod"] = Color.Goldenrod,
      ["brown"] = Color.Brown,
      ["gray"] = Color.Gray,
      ["grey"] = Color.Gray,
      ["ivory"] = Color.Ivory,
      ["violet"] = Color.Violet,
    };
  }    
}