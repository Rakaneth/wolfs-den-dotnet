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

  public class TempBonus: BaseStat, IUpkeep
  {
    public const int FOREVER = -1;
    public int Duration {get; private set;}
    public bool Expired => Duration == 0;
    public TempBonus(int baseVal=0, float mult=0f, int duration=FOREVER): base(baseVal, mult)
    {
      Duration = duration;
    }
    public void Tick()
    {
      if (Duration > 0) 
        Duration--;    
    }
  }


  public class Stat: BaseStat, IUpkeep
  {
    public string Name {get;}
    private List<TempBonus> _rawBonuses = new List<TempBonus>();
    private List<TempBonus> _finalBonuses = new List<TempBonus>();
    private bool _dirty;
    private int _val;

    public int Value
    {
      get
      {
        if (_dirty)
        {
          float acc = BaseValue;
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

    public void AddRawBonus(TempBonus bonus) 
    {
      _rawBonuses.Add(bonus);
      _dirty = true;
    }
    public void AddFinalBonus(TempBonus bonus)
    {
      _finalBonuses.Add(bonus);
      _dirty = true;
    } 
    public void RemoveRawBonus(TempBonus bonus)
    {
      _rawBonuses.Remove(bonus);
      _dirty = true;
    }
    public void RemoveFinalBonus(TempBonus bonus)
    {
      _finalBonuses.Remove(bonus);
      _dirty = true;
    }

    public void SetBaseValue(int val)
    {
      BaseValue = val;
      _dirty = true;
    }

    public void Tick()
    {
      _finalBonuses.RemoveAll( f =>
      {
        f.Tick();
        _dirty = f.Expired;
        return f.Expired;
      });
      _rawBonuses.RemoveAll( r =>
      {
        r.Tick();
        _dirty = r.Expired;
        return r.Expired;
      });
    }
  }
}