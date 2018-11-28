using SadConsole.Controls;

namespace GoRogueTest.UI
{
  public class Label: DrawingSurface
  {
    private string _caption;
    public string Caption 
    {
      get => _caption;
      set
      {
        IsDirty = true;
        _caption = value;
      }
    }
    public Label(int width, int height, string caption): base(width, height)
    {
      Caption = caption;
    }

    public Label(int width, int height): this(width, height, "") {}

    public override void Update(System.TimeSpan time)
    {
      Surface.Clear();
      Surface.Print(0, 0, Caption, Parent.DefaultForeground);
      base.Update(time);
    }
  }
}