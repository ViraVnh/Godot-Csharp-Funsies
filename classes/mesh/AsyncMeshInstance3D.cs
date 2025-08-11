
using System.Threading.Tasks;
using Godot;

namespace Veila.classes.mesh;

/// <summary>
/// Create a MeshInstance3D but with 0 interactions with the node tree.
/// Very efficient for large scale procedural generation, but needs careful tracking
/// to avoid memory leaks.
/// </summary>
[Tool]
public class AsyncMeshInstance3D
{
  /// <summary>
  /// The active Rid instance (rendering instance, != the rid of the mesh or object)
  /// </summary>
  public Rid Rid;

  // Save references to the mesh and transform to ensure they don't get freed by godot while used.
  public Mesh mesh;
  public Transform3D transform;



  public AsyncMeshInstance3D(Mesh mesh, Vector3 position, Rid Scenario)
  {
    this.mesh = mesh;
    Rid = RenderingServer.InstanceCreate2(mesh.GetRid(), Scenario);
    SetPosition(position);
    UpdatePosition();
  }

  /// <summary>
  /// Sets the position of the RidNode. Doesn't update rendering (call updatePosition for that)
  /// </summary>
  public void SetPosition(Vector3 position)
  {
    transform.Origin = position;
    transform.Basis = Basis.Identity;
  }
  /// <summary>
  /// Update the rendered position of the object. /!\ doesn't work if the object isn't being rendered.
  /// </summary>
  public void UpdatePosition()
  {
    RenderingServer.InstanceSetTransform(Rid, transform);
  }

  private bool _visible = true;
  public bool Visible
  {
    get => _visible;
    set
    {
      _visible = value;
      RenderingServer.InstanceSetVisible(Rid, _visible);
    }
  }

  /// <summary>
  /// Unloads the mesh.
  /// WARNING: Freeing an RID can sometime happen at a time where the rendering server still
  /// uses the instance in a frame. Ideally Visible should be set to false first,
  /// the call Unload in a CallDefered or after a Task.Delay(5).
  /// 
  /// </summary>
  public void Unload()
  {
    mesh = null;
    if (!Rid.IsValid) return;
    try // https://github.com/godotengine/godot/issues/103073
    {
      RenderingServer.FreeRid(Rid);
    }
    catch
    {
      Task.Run(() => {
        Task.Delay(10);
        RenderingServer.FreeRid(Rid);
      });
    }
  }
}
