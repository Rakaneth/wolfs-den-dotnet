using System;
using Xunit;
using GoRogueTest.Entity;

namespace GoRogueTest.UnitTests
{
    public class TestTest
    {
        [Fact]
        public void TestRaceTemplates()
        {
            Assert.NotEmpty(RaceTemplates.templates);
            Assert.NotEmpty(TraitTemplates.templates);
            Assert.NotEmpty(EquipTemplates.templates);
        }
    }
}
