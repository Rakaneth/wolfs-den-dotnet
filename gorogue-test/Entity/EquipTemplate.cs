using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.IO;

namespace GoRogueTest.Entity
{
  public static class EquipTemplates
  {
    public readonly static Dictionary<string, EquipTemplate> templates;

    static EquipTemplates()
    {
      using (var input = File.OpenText("data/equip.yml"))
      {
        var deser = new DeserializerBuilder()
          .WithNamingConvention(new CamelCaseNamingConvention())
          .Build();
        templates = deser.Deserialize<Dictionary<string, EquipTemplate>>(new MergingParser(new Parser(input)));
      }
    }

    public class EquipTemplate
    {
      public string Name{ get; set;}
      public string Desc{ get; set;}
      public EquipSlot Slot{ get; set;}
      public Stats Stats {get; set;}
      public int Glyph{get; set;}
      public string Color{get; set;}
      public List<string> Tags{get; set;}
      public bool Material {get; set;}
      public DamageType DamageType{get; set;}
      public EquipType EquipType {get; set;}
    }

    public class Stats
    {
      public int Atp {get; set;}
      public int Dfp {get; set;}
      public int Dmg {get; set;}
      public int Tou {get; set;}
      public int Res{get; set;}
      public int Pwr{get; set;}
      public int Edr{get; set;}
      public int Wil{get; set;}
    }
  }
}