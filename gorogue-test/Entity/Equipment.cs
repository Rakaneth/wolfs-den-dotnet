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

  public enum ItemType
  {
    ITEM,
    EQUIP,
    FOOD,
    HEALING
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

  public class Equipment: ItemBase
  {
    public bool Equipped{get; set;}
    public EquipSlot Slot{get;}
    public int Atp {get; private set;}
    public int Dfp{get; private set;}
    public int Dmg{get; private set;}
    public int Tou{get; private set;}
    public int Res{get; private set;}
    public int Pwr{get; private set;}
    public int Edr{get; private set;}
    public int Wil{get; private set;}
    public int Hardness{get; private set;}
    public EquipType EquipType{get;}

    public Equipment(
      EquipTemplates.EquipTemplate eqpTemp, 
      MaterialTemplates.MaterialTemplate matTemp=null, 
      string id=null): base(id, ItemType.EQUIP, -1)
    {
      Equipped = false;
      Slot = eqpTemp.Slot;
      Desc = eqpTemp.Desc;
      Atp = eqpTemp.Stats.Atp;
      Dfp = eqpTemp.Stats.Dfp;
      Dmg = eqpTemp.Stats.Dmg;
      Tou = eqpTemp.Stats.Tou;
      Res = eqpTemp.Stats.Res;
      Pwr = eqpTemp.Stats.Pwr;
      Edr = eqpTemp.Stats.Edr;
      Wil = eqpTemp.Stats.Wil;
      Name = eqpTemp.Name;
      EquipType = eqpTemp.EquipType;
      
      if (eqpTemp.Material && (matTemp?.Stats.ContainsKey(eqpTemp.EquipType) ?? false))
        applyMaterial(matTemp);
      else if (eqpTemp.Material)
      {
        var matName = matTemp?.Name ?? "nothing";
        throw new System.Exception(
          $"{eqpTemp.Name} must be made of something but can't be made of {matName}");
      }
        
      if (!World.Instance.Configs.UnitTest)
      {
        var anim = new Animated(ID, 1, 1);
        var frame = anim.CreateFrame();
        frame[0].Glyph = eqpTemp.Glyph;
        
        if (eqpTemp.Color != null)
          frame[0].Foreground = Swatch[eqpTemp.Color];
        else if (matTemp?.Color != null)
          frame[0].Foreground = Swatch[matTemp.Color];
        else
          frame[0].Foreground = Color.White;

        frame[0].Background = Color.Transparent;
        DrawEntity = new SadConsole.Entities.Entity(anim);
      }
    }

    public Equipment(string tempID, string matID="none", string itemID=null)
      : this(
        EquipTemplates.templates[tempID], 
        matID == "none" ? null : MaterialTemplates.templates[matID],
        itemID) {} //Test
    
    private void applyMaterial(MaterialTemplates.MaterialTemplate matTemp)
    {
        var stats = matTemp.Stats[EquipType];
        Name = $"{matTemp.Name} {Name}";
        Desc = Desc.Replace("<material>", matTemp.Name);
        Atp += stats.Atp;
        Dfp += stats.Dfp;
        Dmg += stats.Dmg;
        Tou += stats.Tou;
        Res += stats.Res;
        Pwr += stats.Pwr;
        Edr += stats.Edr;
        Wil += stats.Wil;
        Hardness = matTemp.Hardness;
    }

    public override void Use(Actor actor) {}
  }
}