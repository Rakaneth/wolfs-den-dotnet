using GoRogue;
using System.Collections.Generic;
using System.Linq;

namespace GoRogueTest.Entity
{
  public class Actor: GameEntity
  {
    public bool IsPlayer{get; private set;}
    public Actor(string id, bool isPlayer = false): base(id) 
    {
      IsPlayer = isPlayer;
      Layer = IsPlayer ? 3 : 2;
      AddTag(ID);
    }
    public Actor(): base() 
    {
      Layer = 2;
      AddTag(ID);
    }
    private FOV _fov;
    public IEnumerable<Coord> Visible => _fov.CurrentFOV;
    public void MoveBy(int dx, int dy) => Position = Position.Translate(dx, dy);
    public void SetFOV() => _fov = new FOV(Map.ResConverter);
    private List<string> _inventory = new List<string>();
    private HashSet<string> _enemies = new HashSet<string>();
    private HashSet<string> _allies = new HashSet<string>();
    public List<string> Inventory => _inventory;
    public int Capacity {get; set;}
    public bool BagsFull => _inventory.Count >= Capacity;
    public Equipment NaturalWeapon {get; private set;}
    public Stat Strength = new Stat("Strength", 10);
    public Stat Stamina = new Stat("Stamina", 10);
    public Stat Speed = new Stat("Speed", 10);
    public Stat Skill = new Stat("Skill", 10);
    public Stat Sagacity = new Stat("Sagacity", 10);
    public Stat Smarts = new Stat("Smarts", 10);
    public HashSet<Trait> Traits {get; private set;}

    public void UpdateFOV()
    {
      _fov.Calculate(Position, 6.0);
      if (IsPlayer)
        foreach(var pos in Visible)
          Map.Explore(pos);
    }
        

    public bool CanSee(Coord c) => Visible.FirstOrDefault(cand => cand == c) != null;   
    public bool CanSee(GameEntity thing) => CanSee(thing.Position);
    public bool Pickup(GameEntity item)
    {
      if (BagsFull)
      {
        //TODO: message if is the player
        return false;
      }
      else
      {
        //TODO: message if is the player
        _inventory.Add(item.ID);
        item.Position = null;
        item.MapID = null;
        return true;
      }
    } 
    public void Drop(GameEntity item)
    {
      if (item is Equipment)
        Dequip(((Equipment)item).Slot);
      _inventory.Remove(item.ID);
      item.Position = Position;
      item.MapID = MapID;
    }

    public bool HasItem(string itemID) => _inventory.Contains(itemID);

    public Equipment EquippedInSlot(EquipSlot slot)
    {
      return _inventory
        .Select(thing => World.Instance.GetByID<Equipment>(thing))
        .FirstOrDefault(thing => thing.Equipped);
    }

    public void Dequip(EquipSlot slot)
    {
      var curEQ = EquippedInSlot(slot);
      if (curEQ != null)
        curEQ.Equipped = false;
      NaturalWeapon.Equipped = true;
    }

    public void Equip(Equipment item)
    {
      Dequip(item.Slot);
      NaturalWeapon.Equipped = false;
      item.Equipped = true;
    }

    public bool IsEnemy(Actor other) => _enemies.Intersect(other.Tags).Count() > 0;
    public bool IsAlly(Actor other)
    {
      return _allies.Intersect(other.Tags).Count() > 0 && !IsEnemy(other);
    }

    public bool IsNeutral(Actor other) => !(IsEnemy(other) || IsAlly(other));

    public void AddAlly(string allyFaction)
    {
      _allies.Add(allyFaction);
      _enemies.Remove(allyFaction);
    }

    public void MakeNeutral(string neutralFaction)
    {
      _allies.Remove(neutralFaction);
      _enemies.Remove(neutralFaction);
    }
    public void AddEnemy(string enemyFaction)
    {
      _enemies.Add(enemyFaction);
      _allies.Remove(enemyFaction);
    }

    public void ClearAllies() => _allies.Clear();
    public void ClearEnemies() => _enemies.Clear();

    public void MakeCompanion(Actor companion)
    {
      companion.ClearAllies();
      companion.AddAlly(ID);
    }

    public void MakePlayer() => IsPlayer = true;
    public void MakeNPC() => IsPlayer = false;

    public void NewNaturalWeapon(Equipment naturalWeapon)
    {
      NaturalWeapon = naturalWeapon;
    }
  }
}