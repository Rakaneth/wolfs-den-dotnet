using Troschuetz.Random;
using Troschuetz.Random.Generators;
using GoRogue.Random;
using System.Collections.Generic;
using System.Linq;

namespace GoRogueTest.RNG
{
  public class ProbabilityTable<T>
  {
    private IDictionary<T, int> _tbl = new Dictionary<T, int>();

    private int _totalWeights => _tbl.Sum(s => s.Value);

    private IGenerator _rng;

    public ProbabilityTable(IGenerator generator)
    {
      _rng = generator;
    }

    public ProbabilityTable() : this(SingletonRandom.DefaultRNG) {}

    public void Add(T item, int weight)
    {
      _tbl.Add(item, weight);
    }

    public void Remove(T item)
    {
      _tbl.Remove(item);
    }

    public T Get()
    {
      var roll = _rng.Next(_totalWeights);
      int current = 0;
      T result = _tbl.First().Key;
      foreach (var pair in _tbl)
      {
        current += pair.Value;
        if (roll < current)
        {
          result = pair.Key;
          break;
        }
      }
      return result;
    }
  }
}

