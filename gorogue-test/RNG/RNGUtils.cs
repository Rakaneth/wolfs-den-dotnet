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

    public static void Shuffle<T>(this IList<T> list)
    {
      int max = list.Count - 1;
      int roll;
      for (int i=0; i<max; i++)
      {
        roll = World.Instance.RNG.Next(i, list.Count);
        list.Swap(i, roll);
      }
    }

    
    public static T GetByRarity<T>(IDictionary<string, T> dict)
    {
      var tbl = new ProbabilityTable<string>();
      foreach (var cand in dict)
      {
        var castValue = cand.Value as IRarity;
        if (castValue == null)
        {
          throw new System.Exception("Values of dict are not IRarity");
        }
        tbl.Add(cand.Key, castValue.Rarity);
      }
      var result = tbl.Get();
      return dict[result];
    }
  }
}