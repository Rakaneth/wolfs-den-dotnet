using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.IO;

namespace GoRogueTest.Entity
{
  public class RaceTemplates
  {
    public readonly static Dictionary<string, RaceTemplate> templates;
    static RaceTemplates()
    {
      using (var input = File.OpenText("data/races.yml"))
      {
        var deser = new DeserializerBuilder()
          .WithNamingConvention(new CamelCaseNamingConvention())
          .Build();
        templates = deser.Deserialize<Dictionary<string, RaceTemplate>>(new MergingParser(new Parser(input)));
      }
    }

    public class RaceTemplate
    {
      public string Desc {get; set;}
      public Stats BaseStats {get; set;}
      public List<string> StartItems {get; set;}
      public string Info {get; set;}
    }

    public class Stats
    {
      public int Str{get; set;}
      public int Stam{get; set;}
      public int Spd{get; set;}
      public int Skl{get; set;}
      public int Sag{get; set;}
      public int Smt{get; set;}
    }
  }
}