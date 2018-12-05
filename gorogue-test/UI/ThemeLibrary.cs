using SadConsole;
using SadConsole.Themes;
using Microsoft.Xna.Framework;

namespace GoRogueTest.UI
{
  public class ThemeLibrary
  {
    public static RadioButtonTheme BWRadioTheme = new RadioButtonTheme();

    static ThemeLibrary()
    {
      BWRadioTheme.Normal = new Cell(Color.White, Color.Black);
      BWRadioTheme.MouseOver = new Cell(Color.Black, Color.White);
      BWRadioTheme.Disabled = new Cell(Color.DarkGray, Color.Black);
      BWRadioTheme.LeftBracket.Normal = new Cell(Color.White, Color.Black);
      BWRadioTheme.LeftBracket.MouseOver = new Cell(Color.Black, Color.White);
      BWRadioTheme.LeftBracket.Disabled = new Cell(Color.DarkGray, Color.Black);
      BWRadioTheme.LeftBracket.SetGlyph('(');
      BWRadioTheme.RightBracket.Normal = new Cell(Color.White, Color.Black);
      BWRadioTheme.RightBracket.MouseOver = new Cell(Color.Black, Color.White);
      BWRadioTheme.RightBracket.Disabled = new Cell(Color.DarkGray, Color.Black);
      BWRadioTheme.RightBracket.SetGlyph(')');
      BWRadioTheme.CheckedIcon.SetBackground(Color.Black);
      BWRadioTheme.CheckedIcon.SetForeground(Color.White);
      BWRadioTheme.UncheckedIcon.SetBackground(Color.Black);
      BWRadioTheme.CheckedIcon.SetForeground(Color.White);
    }
  }

}