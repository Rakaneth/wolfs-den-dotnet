using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SadConsole.Entities;
using SadConsole.Surfaces;
using static GoRogueTest.UI.UIUtils;

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
    ARMOR,
    NATURAL
  }

  public class Equipment: GameEntity
  {
    public bool Equipped{get; set;}
    public EquipSlot Slot{get;}
    public int Atp {get;}
    public int Dfp{get;}
    public int Dmg{get;}
    public int Tou{get;}
    public int Res{get;}
    public int Pwr{get;}
    public int Edr{get;}
    public int Wil{get;}

    public Equipment(EquipTemplates.EquipTemplate template, string id=null): base(id)
    {
      Equipped = false;
      Slot = template.Slot;
      Name = template.Name;
      Desc = template.Desc;
      Atp = template.Stats.Atp;
      Dfp = template.Stats.Dfp;
      Dmg = template.Stats.Dmg;
      Tou = template.Stats.Tou;
      Res = template.Stats.Res;
      Pwr = template.Stats.Pwr;
      Edr = template.Stats.Edr;
      Wil = template.Stats.Wil;

      var anim = new Animated(ID, 1, 1);
      var frame = anim.CreateFrame();
      frame[0].Glyph = template.Glyph;
      if (template.Color == null)
        frame[0].Foreground = Swatch[template.Color];
      else
        frame[0].Foreground = Color.White;
      frame[0].Background = Color.Transparent;
      DrawEntity = new SadConsole.Entities.Entity(anim);
    }

    public Equipment(string tempID, string itemID=null)
      : this(EquipTemplates.templates[tempID], itemID) {} //Test
  }
}