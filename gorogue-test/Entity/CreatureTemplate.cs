using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.IO;
using GoRogueTest.RNG;

namespace GoRogueTest.Entity
{
  public static class CreatureTemplates
  {
    public readonly static Dictionary<string, CreatureTemplate> templates;
    static CreatureTemplates()
    {
      using (var input = File.OpenText("data/creatures.yml"))
      {
        var deser = new DeserializerBuilder()
          .WithNamingConvention(new CamelCaseNamingConvention())
          .Build();
        templates = deser.Deserialize<Dictionary<string, CreatureTemplate>>(
          new MergingParser(new Parser(input)));
      }
    }

    public class CreatureTemplate: IRarity
    {
      public int Rarity{get; set;}
      public string Desc{get; set;}
      public string Name{get; set;}
      public string Type{get; set;}
      public string Unarmed{get; set;}
      public BaseStats BaseStats{get; set;}
      public List<string> StartInventory {get; set;}
      public List<string> Tags{get; set;}
      public List<string> Allies{get; set;}
      public List<string> Enemies{get; set;}
      public int Capacity{get; set;}
      public int Glyph {get; set;}
      public string Color {get; set;}
    }

    public class BaseStats
    {
      public int Str {get; set;}
      public int Stam{get; set;}
      public int Spd {get; set;}
      public int Skl {get; set;}
      public int Sag{get; set;}
      public int Smt{get; set;}
    }
  }
}