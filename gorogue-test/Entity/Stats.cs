using System;
using System.Collections.Generic;
using System.Linq;

namespace GoRogueTest.Entity
{
  public class BaseStat
  {
    public int BaseValue {get; protected set;}
    public float Mult {get; protected set;}

    public BaseStat(int baseVal=0, float mult=0f)
    {
      BaseValue = baseVal;
      Mult = mult;
    }
  }

  public class RawBonus: BaseStat
  {
    public RawBonus(int baseVal=0, float mult=0f): base(baseVal, mult){}
  }

  public class FinalBonus: BaseStat
  {
    public FinalBonus(int baseVal=0, float mult=0f): base(baseVal, mult){}
  }

  public class Stat: BaseStat
  {
    public string Name {get;}
    private List<RawBonus> _rawBonuses = new List<RawBonus>();
    private List<FinalBonus> _finalBonuses = new List<FinalBonus>();
    private bool _dirty;
    private int _val;

    public int Value
    {
      get
      {
        float acc = BaseValue;
        if (_dirty)
        {
          int flats = _rawBonuses.Sum(s => s.BaseValue);
          float mults = _rawBonuses.Sum(s => s.Mult);
          int finalFlats = _finalBonuses.Sum(s => s.BaseValue);
          float finalMults = _finalBonuses.Sum(s => s.Mult);
          acc *= (1 + mults);
          acc += flats;
          acc *= (1 + finalMults);
          acc += finalFlats;
          _val = (int)acc;
          _dirty = false;
        }
        return _val;
      }
    }

    public Stat(string name, int startVal): base(startVal)
    {
      _val = startVal;
      _dirty = true;
      Name = name;
    }

    public void AddRawBonus(RawBonus bonus) 
    {
      _rawBonuses.Add(bonus);
      _dirty = true;
    }
    public void AddFinalBonus(FinalBonus bonus)
    {
      _finalBonuses.Add(bonus);
      _dirty = true;
    } 
    public void RemoveRawBonus(RawBonus bonus)
    {
      _rawBonuses.Remove(bonus);
      _dirty = true;
    }
    public void RemoveFinalBonus(FinalBonus bonus)
    {
      _finalBonuses.Remove(bonus);
      _dirty = true;
    }

    public void SetBaseValue(int val)
    {
      BaseValue = val;
      _dirty = true;
    }
  }
}