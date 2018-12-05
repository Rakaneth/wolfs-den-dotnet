using GoRogue;
using System.Collections.Generic;
using System.Linq;

namespace GoRogueTest.Entity
{
  public class Actor: GameEntity
  {
    public bool IsPlayer{get;}
    public Actor(string id, bool isPlayer = false): base(id) 
    {
      IsPlayer = isPlayer;
    }
    public Actor(): base() {}
    private FOV _fov;
    public IEnumerable<Coord> Visible => _fov.CurrentFOV;
    public void MoveBy(int dx, int dy) => Position = Position.Translate(dx, dy);
    public void SetFOV() => _fov = new FOV(Map.ResConverter);
    private List<string> _inventory = new List<string>();
    public List<string> Inventory => _inventory;
    public int Capacity {get; set;}
    public bool BagsFull => _inventory.Count >= Capacity;
    private Equipment _naturalWeapon;
    public Equipment NaturalWeapon => _naturalWeapon;

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
    }

    public void Equip(Equipment item)
    {
      Dequip(item.Slot);
      item.Equipped = true;
    }
  }

}