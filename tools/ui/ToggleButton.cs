//
//  Quick toggle button that switches a target control's visibility.
//  this makes for a quick and dirty menu opener, good for debug.
//  For large menus scene instantiation is probably best.
//

using Godot;

public partial class ToggleButton: Button
{

  [Export] public Control target;

  public override void _Ready()
  {
    Pressed += Toggle;
  }

  public void Toggle()
  {
    if (target is not null)
    {
      if (target.Visible) target.Hide();
      else                target.Show();
    }
  }

  public override void _ExitTree()
  {
    Pressed -= Toggle;
  }

}
