using System;
using Xunit;
using GoRogueTest.Entity;
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
    [Fact]
    public void TestProbTable()
    {
      
    }
  }
}
