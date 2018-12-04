using System.Collections.Generic;
using System;

namespace GoRogueTest
{
  public class PrioQueue<T> where T: IComparable
  {
    private List<T> _data;

    public PrioQueue()
    {
      _data = new List<T>();
    }

    public PrioQueue(IEnumerable<T> elements)
    {

    }

    private void swap(int idxA, int idxB)
    {
      var temp = _data[idxA];
      _data[idxA] = _data[idxB];
      _data[idxB] = temp;
    }

    private void siftDown(T element)
    {
      _data.Add(element);
      if (_data.Count < 1)
      {
        return;
      }

      int i = _data.Count - 1;

      while (_data[i].CompareTo(_data[i-1]) < 0 && i > 0)
      {
        swap(i-1, i);
        i--;
      }
    }

  }
}