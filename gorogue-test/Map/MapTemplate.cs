using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GoRogueTest.Map
{
  public static class MapTemplates
  {
    public static readonly IDictionary<string, MapTemplate> templates;

    static MapTemplates()
    {
      using (var input = File.OpenText("data/maps.yml"))
      {
        var deser = new DeserializerBuilder()
          .WithNamingConvention(new CamelCaseNamingConvention())
          .Build();
        templates = deser.Deserialize<Dictionary <string, MapTemplate>>(
          new MergingParser(new Parser(input)));
      }
    }

    public class MapTemplate
    {
      public int Width{get; set;}
      public int Height {get; set;}
      public bool Light {get; set;}
      public string Type {get; set;}
      public string Name {get; set;}
      public List<MapConnection> Connections {get; set;}
    }
  }
  
}