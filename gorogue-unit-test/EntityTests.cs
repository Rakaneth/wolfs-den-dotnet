using System;
using Xunit;
using Xunit.Abstractions;
using GoRogueTest.Entity;
using GoRogueTest.RNG;
using GoRogue.Random;
using System.Collections.Generic;

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
        list.Shuffle(SingletonRandom.DefaultRNG);
        output.WriteLine($"Shuffled list [{i+1}]: {string.Join(',', list)}");
      }
    }

    [Theory]
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
  }



  
}
