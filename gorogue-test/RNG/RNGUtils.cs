using GoRogue.Random;
using Troschuetz.Random;
using System.Collections.Generic;

namespace GoRogueTest.RNG
{
  public static class RNGUtils
  {

    public static void Swap<T>(this IList<T> list, int idxA, int idxB)
    {
      var tmp = list[idxA];
      list[idxA] = list[idxB];
      list[idxB] = tmp;
    }

    public static void Shuffle<T>(this IList<T> list, IGenerator rng)
    {
      int max = list.Count - 1;
      int roll;
      for (int i=0; i<max; i++)
      {
        roll = rng.Next(i, list.Count);
        list.Swap(i, roll);
      }
    }
  }
}