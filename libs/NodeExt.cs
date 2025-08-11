
using Godot;

namespace Veila.libs;

[Tool]
public static class NodeExt
{
  [Tool]
  public static class Scenes
  {
    /// <summary>
    /// Instantiate a scene from code
    /// </summary>
    public static bool Load(string path, Node parent = null)
    {
      if (!ResourceLoader.Exists(path)) return false;

      PackedScene scene = ResourceLoader.Load<PackedScene>(path);
      Node root = scene.Instantiate();
      
      if (parent is not null)
      {
        parent.AddChild(root);
        root.Owner = parent;
      }

      return true;
    }
  }

  [Tool]
  public static class Nodes
  {
    /// <summary>
    /// Attach a child to a node with proper in editor support
    /// </summary>
    public static void AddChild(Node node, Node to)
    {
      to.AddChild(node);
      node.Owner = to.GetTree().Root;
      #if TOOLS
      node.Owner = to.GetTree().EditedSceneRoot;
      #endif
    }

    /// <summary>
    /// Attach a child to a node with proper in editor support
    /// </summary>
    public static void AddChild(Node2D node, Node2D to)
    {
      to.AddChild(node);
      node.Owner = to.GetTree().Root;
      #if TOOLS
      node.Owner = to.GetTree().EditedSceneRoot;
      #endif
    }

    /// <summary>
    /// Attach a child to a node with proper in editor support
    /// </summary>
    public static void AddChild(Node3D node, Node3D to)
    {
      to.AddChild(node);
      node.Owner = to.GetTree().Root;
      #if TOOLS
      node.Owner = to.GetTree().EditedSceneRoot;
      #endif
    }

    /// <summary>
    /// Attach a Node3D to a Node2D with proper in editor support.
    /// You might want to introduce coordinate tracking logic (see NodeExt.Nodes.Track).
    /// </summary>
    public static void AddChild(Node3D node, Node2D to)
    {
      to.AddChild(node);
      node.Owner = to.GetTree().Root;
      #if TOOLS
      node.Owner = to.GetTree().EditedSceneRoot;
      #endif
      Track(node, to);
    }

    /// <summary>
    /// Attach a Node2D to a Node3D with proper in editor support.
    /// You might want to introduce coordinate tracking logic (see NodeExt.Nodes.Track).
    /// </summary>
    public static void AddChild(Node2D node, Node3D to)
    {
      to.AddChild(node);
      node.Owner = to.GetTree().Root;
      #if TOOLS
      node.Owner = to.GetTree().EditedSceneRoot;
      #endif
      Track(node, to);
    }

    /// <summary>
    /// Relocate a Node3D based on the position of a Node2D.
    /// </summary>
    public static void Track(Node3D node, Node2D target)
    {
      // TODO: introduce axis support.
      node.Position = new(
        target.Position.X,
        node.Position.Y,
        target.Position.Y
      );
    }

    /// <summary>
    /// Relocate a Node2D based on the position of a Node3D.
    /// </summary>
    public static void Track(Node2D node, Node3D target)
    {
      // TODO: introduce axis support.
      node.Position = new(
        target.Position.X,
        target.Position.Z
      );
    }

    /// <summary>
    /// Properly remove a child from the scene tree and frees it.
    /// </summary>
    public static void FreeChild(Node node, Node from)
    {
      from.RemoveChild(node);
      node.Owner = null;
      node.QueueFree();
    }
  }
}
