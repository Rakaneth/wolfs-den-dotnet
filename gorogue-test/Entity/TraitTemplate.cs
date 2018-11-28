using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoRogueTest.Entity
{
  public class TraitTemplates
  {
    public static readonly Dictionary<string, TraitTemplate> templates = new Dictionary<string, TraitTemplate>();

    static TraitTemplates()
    {
      using (var input = File.OpenText("data/traits.yml"))
      {
        var deser = new DeserializerBuilder()
          .WithNamingConvention(new CamelCaseNamingConvention())
          .Build();
        templates = deser.Deserialize<Dictionary<string, TraitTemplate>>(new MergingParser(new Parser(input)));
      }
    }

    public static Dictionary<string, TraitTemplate> getByType(string tp)
    {
      return templates
        .Where(t => t.Value.Type == tp)
        .OrderBy(o => o.Key)
        as Dictionary<string, TraitTemplate>;
    }

    public class TraitTemplate
    {
      public string Desc{ get; set; }
      public string Type{get; set;}
      public string Info {get; set;}
      public Stats BonusStats{get; set;}
      public List<string> StartEquip{get; set;}
      public string BaseAttack {get; set;}
    }

    public class Stats
    {
      public int Str {get; set;}
      public int Stam {get; set;}
      public int Spd {get; set;}
      public int Skl {get; set;}
      public int Sag {get; set; }
      public int Smt {get; set;}
    }
  }
}