using GoRogue;
using System.Collections.Generic;
using System;

namespace GoRogueTest.Entity
{
  public class ActorBuilder
  {
    private Actor _foetus;

    public ActorBuilder(string id=null) 
    {
      if (id != null)
        _foetus = new Actor(id);
      else
        _foetus = new Actor();
    }

    public ActorBuilder WithPosition(Coord c)
    {
      _foetus.Position = c;
      return this;
    }

    public ActorBuilder WithPosition(int x, int y)
    {
      return WithPosition(Coord.Get(x, y));
    }

    public ActorBuilder WithStartMap(string mapID)
    {
      _foetus.MapID = mapID;
      return this;
    }

    public ActorBuilder WithName(string name)
    {
      _foetus.Name = name;
      return this;
    }

    public ActorBuilder WithDesc(string desc)
    {
      _foetus.Desc = desc;
      return this;
    }

    public ActorBuilder WithEntity(SadConsole.Entities.Entity entity)
    {
      _foetus.DrawEntity = entity;
      return this;
    }

    public ActorBuilder MakePlayer()
    {
      _foetus.MakePlayer();
      return this;
    }

    public ActorBuilder WithCapacity(int capacity)
    {
      _foetus.Capacity = capacity;
      return this;
    }

    public ActorBuilder WithAllies(IEnumerable<string> allies)
    {
      if (allies != null)
      {
        foreach (var ally in allies)
          _foetus.AddAlly(ally);
      }
      return this;
    }

    public ActorBuilder WithAllies(params string[] allies) => WithAllies(allies);

    public ActorBuilder WithEnemies(IEnumerable<string> enemies)
    {
      if (enemies != null)
      {
        foreach (var enemy in enemies)
          _foetus.AddEnemy(enemy);
      }
      return this;
    }

    public ActorBuilder WithEnemies(params string[] enemies) => WithEnemies(enemies);

    public ActorBuilder WithStr(int str)
    {
      _foetus.Strength.SetBaseValue(str);
      return this;
    }

    public ActorBuilder WithStam(int stam)
    {
      _foetus.Stamina.SetBaseValue(stam);
      return this;
    }

    public ActorBuilder WithSpd(int spd)
    {
      _foetus.Speed.SetBaseValue(spd);
      return this;
    }

    public ActorBuilder WithSkl(int skl)
    {
      _foetus.Skill.SetBaseValue(skl);
      return this;
    }

    public ActorBuilder WithSag(int sag)
    {
      _foetus.Sagacity.SetBaseValue(sag);
      return this;
    }

    public ActorBuilder WithSmt(int smt)
    {
      _foetus.Smarts.SetBaseValue(smt);
      return this;
    }

    public ActorBuilder WithTags(IEnumerable<string> tags)
    {
      foreach (string tag in tags)
        _foetus.AddTag(tag);
      return this;
    }

    public ActorBuilder AddRace(RaceTemplates.RaceTemplate temp)
    {
      _foetus.Desc = temp.Desc;
      WithStr(temp.BaseStats.Str);
      WithStam(temp.BaseStats.Stam);
      WithSpd(temp.BaseStats.Spd);
      WithSkl(temp.BaseStats.Skl);
      WithSag(temp.BaseStats.Sag);
      WithSmt(temp.BaseStats.Smt);
      
      foreach(string tag in temp.Tags)
        _foetus.AddTag(tag);
      
      return this;
    }

    public ActorBuilder WithType(string type)
    {
      _foetus.SetType(type);
      return this;
    }

    public ActorBuilder WithTraits(IEnumerable<Trait> traits)
    {
      //TODO: Traits
      throw new NotImplementedException();
      //return this;
    }

    public ActorBuilder WithGear(IEnumerable<string> itemIDs)
    {
      //TODO: ItemBuilder / Gear
      throw new NotImplementedException();
      //return this;
    }


    public Actor Build()
    {
      if (!World.Instance.Configs.UnitTest && _foetus.Map != null)
      {
        _foetus.SetFOV();
        _foetus.UpdateFOV();
      }
      return _foetus;
    }

    public static Actor FromTemplate(
      CreatureTemplates.CreatureTemplate temp,
      RaceTemplates.RaceTemplate race = null,
      string ID = null,
      bool isPlayer = false)
    {
      ActorBuilder result = new ActorBuilder(ID);
      if (temp.Type == "person" && race == null)
      {
        var raceTemp = RNG.RNGUtils
          .GetByRarity<RaceTemplates.RaceTemplate>(RaceTemplates.templates);
        result.AddRace(raceTemp);
      }
      else if (race != null)
      {
        result.AddRace(race);
      }
      else
      {
        result.WithStr(temp.BaseStats.Str)
          .WithStam(temp.BaseStats.Stam)
          .WithSpd(temp.BaseStats.Spd)
          .WithSkl(temp.BaseStats.Skl)
          .WithSag(temp.BaseStats.Sag)
          .WithSmt(temp.BaseStats.Smt)
          .WithName(temp.Name)
          .WithDesc(temp.Desc)
          .WithAllies(temp.Allies)
          .WithEnemies(temp.Enemies)
          .WithTags(temp.Tags)
          .WithCapacity(Math.Max(temp.Capacity, 1))
          .WithType(temp.Type);
      }
      if (isPlayer)
        result.MakePlayer();
      return result.Build();
    }

    public static Actor FromTemplate(string buildID)
    {
      return FromTemplate(CreatureTemplates.templates[buildID]);
    }
  }


}