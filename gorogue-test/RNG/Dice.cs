using GoRogue.DiceNotation;
using System;

namespace GoRogueTest.RNG
{
  public enum DiceDifficulty
  {
    STANDARD = 60,
    CHALLENGING = 70,
    DIFFICULT = 85,
    IMPROBABLE = 100,
    IMPOSSIBLE = 110
  }

  public class DiceResults
  {
    public int Margin {get;}
    public bool Success {get;}

    public DiceResults(int margin, bool success)
    {
      Margin = margin;
      Success = success;
    }
  }
  public static class DiceRolls
  {
    public static IDiceExpression PERCENT = Dice.Parse("1d100");

    public static DiceResults BasicCheck(int skl, DiceDifficulty diff=DiceDifficulty.STANDARD)
    {
      int roll = skl + PERCENT.Roll(World.Instance.RNG);
      int margin = roll - (int)diff;
      bool sux = margin > 0;
      string debugMsg = $"Roll of {roll} vs diff {diff} ({(int)diff}): {(sux ? "Success" : "Failure")}({margin})";
      Logger.Log(LogLevel.INFO, debugMsg);
      return new DiceResults(Math.Abs(margin), sux);
    }
  }
}