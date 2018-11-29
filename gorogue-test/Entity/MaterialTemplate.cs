using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.IO;

namespace GoRogueTest.Entity
{
  public class MaterialTemplates
  {
    public readonly static Dictionary<string, MaterialTemplate> templates;

    static MaterialTemplates()
    {
      using (var input = File.OpenText("data/materials.yml"))
      {
        var deser = new DeserializerBuilder()
          .WithNamingConvention(new CamelCaseNamingConvention())
          .Build();
        templates = deser.Deserialize<Dictionary<string, MaterialTemplate>>(new MergingParser(new Parser(input)));
      }
    }

    public class Stats
    {
      public int Atp {get; set;}
      public int Dfp {get; set;}
      public int Tou {get; set;}
      public int Res {get; set;}
      public int Pwr{get; set;}
      public int Wil{get; set;}
      public int Dmg{get; set;}
    }

    public class MaterialTemplate
    {
      public string Name {get; set;}
      public string Color{get; set;}
      public int Hardness{get; set;}
      public Dictionary<string, Stats> Stats {get; set;}
    }
  }




}