using System;
using Xunit;
using Xunit.Abstractions;
using GoRogueTest.Entity;
using GoRogueTest.RNG;
using GoRogue.Random;

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

    [Theory(Skip="Run this manually")]
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
  }
}
