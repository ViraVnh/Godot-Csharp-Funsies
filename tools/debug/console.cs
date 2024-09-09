using Godot;

// Add a namespace ? <=

public partial class Console: RichTextLabel
{
  
  [Export] public LineEdit input;
  RegEx regex = new();

  public static Console _;
  public Console() { _ = this; }

  override public void _Ready()
  {
    regex.Compile("\\[.*?\\]");

    if (input is null)
    {
      AppendText($"\ninitialization failed: input not found.");
      return;
    }

    input.TextSubmitted += handler;
  }

  override public void _ExitTree()
  {
    if (input is not null) input.TextSubmitted -= handler;
  }

  public void echo(string text)
  {
    AppendText($"\n\t{text}\n");
  }

  private void handler(string text)
  {
    input.Clear();

    AppendText($"\n[color=gray]> {regex.Sub(text, "", true)}[/color]");
    AppendText($"\n\t{Evaluate(text)}\n");
  }

  public Variant Evaluate(string command)
  {
    string cmd = command;
    GodotObject ctx = _;

    Expression exp = new();
    var err = exp.Parse(cmd);
    if (err != Error.Ok) return $"\n\t[color=red]{exp.GetErrorText()}[/color]";

    try
    {
      var res = exp.Execute(null, ctx);
      if (!exp.HasExecuteFailed()) return res;
      
      return $"\n\t[color=red]{exp.GetErrorText()}[/color]";
    } finally {}
  }

}
