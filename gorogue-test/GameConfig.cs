namespace GoRogueTest
{
  public class GameConfigs
  {
    public bool UnitTest {get; set;}
    public bool Logging  {get; set;}

    public GameConfigs(bool unittest=false, bool logging=true)
    {
      UnitTest = unittest;
      Logging = logging;
    }
  }
}