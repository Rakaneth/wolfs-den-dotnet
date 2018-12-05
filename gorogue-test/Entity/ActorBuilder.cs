using GoRogue;

namespace GoRogueTest.Entity
{
  public class ActorBuilder
  {
    private Actor _foetus;
    private Coord _startPos;
    private string _startMap;
    private string _name;
    private string _id;
    private SadConsole.Entities.Entity _entity;
    private bool _isPlayer;

    public ActorBuilder() {}

    public ActorBuilder WithID(string id)
    {
      _id = id;
      return this;
    }

    public ActorBuilder WithPosition(Coord c)
    {
      _startPos = c;
      return this;
    }

    public ActorBuilder WithPosition(int x, int y)
    {
      return WithPosition(Coord.Get(x, y));
    }

    public ActorBuilder WithStartMap(string mapID)
    {
      _startMap = mapID;
      return this;
    }

    public ActorBuilder WithName(string name)
    {
      _name = name;
      return this;
    }

    public ActorBuilder WithEntity(SadConsole.Entities.Entity entity)
    {
      _entity = entity;
      return this;
    }

    public ActorBuilder MakePlayer()
    {
      _isPlayer = true;
      return this;
    }

    public void Reset()
    {
      _startMap = null;
      _startPos = null;
      _name = null;
      _entity = null;
      _id = null;
      _isPlayer = false;
    }

    public Actor Build()
    {
      _foetus = new Actor(_id, _isPlayer);
      _foetus.Position = _startPos;
      _foetus.MapID = _startMap;
      _foetus.Name = _name;
      _foetus.DrawEntity = _entity;
      if (_foetus.Map != null)
      {
        _foetus.SetFOV();
        _foetus.UpdateFOV();
      }
      return _foetus;
    }
  }
}