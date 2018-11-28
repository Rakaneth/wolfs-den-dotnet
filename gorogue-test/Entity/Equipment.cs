using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GoRogueTest.Entity
{
  public enum EquipSlot
  {
    HEAD,
    BODY,
    TRINKET,
    WEAPON
  }

  public enum DamageType
  {
    SLASH,
    PIERCE,
    BLUNT,
    MAGIC
  }

  public enum EquipType
  {
    AXE,
    SWORD,
    STAFF,
    RAPIER,
    HAMMER,
    ARMOR
  }

  public class Material
  {
    string name;
    Color color;
    int atp;
    int dfp;
    int dmg;
    int tou;
    int res;
    int pwr;
    int edr;
    int wil;
    List<EquipType> appTypes;
    public int Atp => atp;
    public int Dfp => dfp;
    public int Dmg => dmg;
    public int Tou => tou;
    public int Res => res;
    public int Pwr => pwr;
    public int Edr => edr;
    public int Wil => wil;
    public List<EquipType> AppTypes => appTypes;


  }
}