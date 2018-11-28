using System.Collections.Generic;

namespace GoRogueTest.Entity
{
  public enum EquipSlot
  {
    HEAD,
    BODY,
    TRINKET,
    WEAPON
  }

  public class Material
  {
    public string Name{get;}
    public int Hardness{get;}
    public static Dictionary<string, Material> Materials = new Dictionary<string, Material>() 
    {
      {"wood", new Material("wood", 5)},
      {"leather", new Material("leather", 10)},
      {"flesh", new Material("flesh", 7)},
      {"iron", new Material("iron", 15)},
      {"bone", new Material("bone", 10)},
      {"steel", new Material("steel", 30)},
      {"stone", new Material("stone", 18)},
      {"blackiron", new Material("blackiron", 50)}
    };

    public Material(string name, int hardness)
    {
      Name = name;
      Hardness = hardness;
    }
  }
}