using GoRogue;
using System.Collections.Generic;

namespace GoRogueTest.Entity
{
  public class StatBuffArgs: EffectArgs
  {
    public BaseStat BuffOrDebuff{get;}
    public string StatName {get; }
    public StatBuffArgs(string whichStat, BaseStat buffOrDebuff)
    {
      BuffOrDebuff = buffOrDebuff;
      StatName = whichStat;
    }
  }

  public abstract class BuffBase: Effect<StatBuffArgs>
  {
    protected Actor actor;
    protected string shortStat;
    protected bool isFinal;
    public BuffBase(Actor actor, string stat, string name, int duration, bool isFinal): base(name, duration) 
    {
      this.actor = actor;
      this.shortStat = stat;
      this.isFinal = isFinal;
    }
  }
  public class ApplyBuff: BuffBase
  {
    private readonly IDictionary<string, Stat> statMap= new Dictionary<string, Stat>();
    public ApplyBuff(Actor actor, string whichStat, bool isFinal)
      : base(actor, whichStat, "Apply Buff", INSTANT, isFinal) {}
    

    protected override void OnTrigger(StatBuffArgs e)
    {
      
    }
  }

  public class RemoveBuff: BuffBase
  {
    public RemoveBuff(Actor actor, string whichStat, int duration, bool isFinal)
      : base(actor, whichStat, "Remove Buff", duration, isFinal) {}

    protected override void OnTrigger(StatBuffArgs e)
    {
      if (Duration == 1)
      {
        Stat buff;
        switch(shortStat)
        {
          case "str": buff = actor.Strength; break;
          case "stam": buff = actor.Stamina; break;
          case "spd": buff = actor.Speed; break;
          case "skl": buff = actor.Skill; break;
          case "sag": buff = actor.Sagacity; break;
          case "smt": buff = actor.Smarts; break;
          default: buff = null; break;
        }
        
      }
    }

  }
}


