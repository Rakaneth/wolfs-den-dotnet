namespace GoRogueTest.Entity
{
  public abstract class ItemBase: GameEntity
  {
    public ItemType ItemType {get; private set;}
    public int Uses {get; private set;}

    public ItemBase(string ID, ItemType t, int uses = 1): base(ID)
    {
      ItemType = t;
      Uses = uses;
      Layer = 1;
    }

    public abstract void Use(Actor actor);
  }

}