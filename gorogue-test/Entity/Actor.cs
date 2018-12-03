namespace GoRogueTest.Entity
{
  public class Actor: GameEntity
  {
    public void MoveBy(int dx, int dy) => Position = Position.Translate(dx, dy);
  }
}