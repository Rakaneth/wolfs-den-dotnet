#undef MAIN

using Xunit;
using Xunit.Abstractions;
using GoRogueTest.Entity;
using GoRogueTest.RNG;
using GoRogueTest.Map;
using GoRogue.MapGeneration.Generators;
using GoRogue.MapViews;
using System;

namespace GoRogueTest.UnitTests
{
  public class DataTests
  {
    [Fact]
    public void TestRaceTemplates()
    {
      Assert.NotEmpty(RaceTemplates.templates);
    }

    [Fact]
    public void TestTraitTemplates()
    {
      Assert.NotEmpty(TraitTemplates.templates);
    }

    [Fact]
    public void TestEquipTemplates()
    {
      Assert.NotEmpty(EquipTemplates.templates);
    }

    [Fact]
    public void TestMaterialTemplates()
    {
      Assert.NotEmpty(MaterialTemplates.templates);
    }
  }

  public class RandomTests
  {
    private readonly ITestOutputHelper output;
    public RandomTests(ITestOutputHelper output)
    {
      this.output = output;
      World.Create(0xDEADBEEF, new GameConfigs(true));
    }

    [Theory(Skip="Run manually")]
    [InlineData(1000)]
    [InlineData(10000)]
    [InlineData(100000)]
    public void TestProbTable(int tries)
    {
      var prob = new ProbabilityTable<string>();
      int common = 0;
      int uncommon = 0;
      int rare = 0;
      prob.Add("common", 12);
      prob.Add("uncommon", 10);
      prob.Add("rare", 8);
      string result;
      for (int i=0; i<tries; i++)
      {
        result = prob.Get();
        switch(result)
        {
          case "common": common++; break;
          case "uncommon": uncommon++; break;
          case "rare": rare++; break;
        }
      }
      int commonStat = common * 30 / tries;
      int uncommonStat = uncommon * 30 / tries;
      int rareStat = rare * 30 / tries;
      string commString = $"Common: {common} - approx: {commonStat}/30";
      string uncommString = $"Uncommon: {uncommon} - approx: {uncommonStat}/30";
      string rareString = $"Rare: {rare} - approx: {rareStat}/30";
      output.WriteLine(commString);
      output.WriteLine(uncommString);
      output.WriteLine(rareString);
    }

    [Theory(Skip="Run manually")]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(10)]
    public void TestShuffle(int tries)
    {
      var list = new int[]{1, 2, 3, 4, 5, 6, 7, 8};
      output.WriteLine($"Original list: {string.Join(',', list)}");
      
      for (int i=0; i<tries; i++)
      {
        list.Shuffle();
        output.WriteLine($"Shuffled list [{i+1}]: {string.Join(',', list)}");
      }
    }

    [Theory(Skip="Run manually")]
    [InlineData(3)]
    public void TestRarity(int tries)
    {
      for (int i=0; i<tries; i++)
      {
        var equip = RNGUtils.GetByRarity(EquipTemplates.templates);
        var race = RNGUtils.GetByRarity(RaceTemplates.templates);
        var material = RNGUtils.GetByRarity(MaterialTemplates.templates);
        string divider = "-----";
        output.WriteLine(divider);
        output.WriteLine($"Equip choice: {equip.Name}");
        output.WriteLine($"Race choice: {race.Desc}");
        output.WriteLine($"Material Choice: {material.Name}");
        output.WriteLine(divider);
      }
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    [InlineData(40)]
    [InlineData(50)]
    public void TestSkillChecks(int skl)
    {
      int sux = 0;
      DiceResults results;
      foreach (DiceDifficulty d in Enum.GetValues(typeof(DiceDifficulty)))
      {
        sux = 0;
        for (int i=0; i<10; i++)
        {
          results = DiceRolls.BasicCheck(skl, d);
          if (results.Success)
            sux++;
        }
        Logger.Log(LogLevel.WARNING, $"{sux}/10 successes at difficulty {d} and skill {skl}");
      }
    }
  }

  public class MapTests
  {
    private ArrayMap<Tile> map = new ArrayMap<Tile>(30, 30);
    private readonly ITestOutputHelper output;

    public MapTests(ITestOutputHelper output)
    {
      this.output = output;
    }

    [Fact]
    public void TestMapGen()
    {
      var transMap = new SetTileMapTranslator(map);
      RectangleMapGenerator.Generate(transMap);
      output.WriteLine(map[0, 0].ToString());
      output.WriteLine(map[1, 1].ToString());
      Assert.False(transMap[0, 0]);
      Assert.True(transMap[1, 1]);
    }
  }

  public class EquipTests
  {
    private readonly ITestOutputHelper output;

    public EquipTests(ITestOutputHelper output)
    {
      this.output = output;
      World.Create(new GameConfigs(true));
    }

    [Fact]
    public void TestBaseEquip()
    {
      var ironAxe = new Equipment("axe", "iron");
      var blueMark = new Equipment("blueMark");
      output.WriteLine(ironAxe.Desc);
      Assert.Equal(12, ironAxe.Dmg);
      Assert.Equal("iron axe", ironAxe.Name);
      Assert.Equal("Mark of Atara", blueMark.Name);
      Assert.Throws<System.Exception>(() => new Equipment("axe", "bone"));
      Assert.Throws<System.Exception>(() => new Equipment("sword"));
    }

    [Fact]
    public void TestStats()
    {
      var dex = new Stat("Dex", 6);
      Assert.Equal(6, dex.BaseValue);
      Assert.Equal(6, dex.Value);
      var flatRaw = new TempBonus(3);
      var multRaw = new TempBonus(0, 0.5f);
      var flatFinal = new TempBonus(8);
      var multFinal = new TempBonus(0, 0.2f);
      dex.AddRawBonus(flatRaw);
      Assert.Equal(9, dex.Value);
      dex.AddRawBonus(multRaw);
      Assert.Equal(12, dex.Value);
      dex.AddFinalBonus(flatFinal);
      Assert.Equal(20, dex.Value);
      dex.AddFinalBonus(multFinal);
      Assert.Equal(22, dex.Value);
      dex.RemoveRawBonus(multRaw);
      Assert.Equal(18, dex.Value);
      dex.RemoveFinalBonus(flatFinal);
      Assert.Equal(10, dex.Value);
      dex.RemoveRawBonus(flatRaw);
      Assert.Equal(7, dex.Value);
      dex.RemoveFinalBonus(multFinal);
      Assert.Equal(6, dex.Value);
    }

    [Fact]
    public void TestStatDurations()
    {
      Stat str = new Stat("Strength", 10);
      str.AddFinalBonus(new TempBonus(baseVal: 5, duration: 5));
      Assert.Equal(15, str.Value);
      str.Tick();
      Assert.Equal(15, str.Value);
      for (int i=0; i<4; i++)
        str.Tick();
      Assert.Equal(10, str.Value);
    }
  }

  public class ActorTests
  {
    [Fact]
    public void TestActorBuilder()
    {
      Actor wolf = ActorBuilder.FromTemplate("wolf");
      Assert.Equal("wolf", wolf.Name);
      Assert.Equal(15, wolf.Strength.Value);
      Assert.Equal(15, wolf.Speed.Value);
    }

  }
}
