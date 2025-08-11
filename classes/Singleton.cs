using Godot;

/**
 * STATUS: Experimental
 * Create autoload-like C# singleton that can be made fully independent from the node tree.
 * Extremely usefull to create a scoped asynchronous "autoload" (eg. per-save terrain generator).
 * For synchronous stuff prefer a simple node extending manager.
 **/

namespace Veila.classes;

/// <summary>
/// Common singleton properties
/// </summary>
public interface ISingleton
{
  void OnLoad();
  void Unload();
  void OnUnload();
}

/// <summary>
/// Create a classic free floating C# singleton but with no instantiation.
/// </summary>
/// <typeparam name="T">The singleton extending class itself here</typeparam>
[Tool]
public abstract class Singleton<T> : ISingleton
  where T : Singleton<T>, new()
{
  private static T _instance = null;
  public  static T _ { get => _instance; }

  public Singleton()
  {
    if (_instance is not null)
    {
      GD.PrintRich($"[color=tomato][hint=\"Instance might not be properly unloaded (memory leak)\"][WARNING] Singleton<{typeof(T)}>: Duplicate singleton instance detected.[/hint][/color]");
      GD.PushWarning($"Singleton<{typeof(T)}>: Duplicate singleton instance detected.");
    }

    _instance = (T)this;
  }

  public abstract void OnLoad();
  public abstract void OnUnload();

  public void Unload()
  {
    OnUnload();
    if (_instance.Equals(this)) _instance = null;
  }
}

/// <summary>
/// Define a loading/reloading point for the singleton.
/// This allows traditional C# singletons to behave like autoloads with a custom
/// attachment point.
/// 
/// /!\ only one singleton context should be loaded at a time at any point.
/// </summary>
/// <typeparam name="T">The singleton extending class</typeparam>
[Tool]
public partial class SingletonContext<T> : Node
  where T : ISingleton, new()
{
  protected T _instance = new();
  public override void _EnterTree() => _instance?.OnLoad();
  public override void _ExitTree()  => _instance?.Unload();
}
