/**
 * STATUS: WIP
 * Mostly work, but misses a lot of features.
 * Note: I almost never use surfaces as resource so I can't track what work and what doesn't on those.
 * I mostly just translate the C# native collections and hope for the best.
 */

using Godot;
using Godot.Collections;

namespace Veila.classes.mesh;

/// <summary>
/// Handles a mesh surface as its own resource.
/// Handy in procedural mesh generation.
/// 
/// /!\ For performance this has been decoupled from Godot's native arrays to use
/// C# Lists instead. If you want a resource compatible see SurfaceResource.
/// </summary>
[Tool, GlobalClass]
public partial class SurfaceResource: Resource
{
  [Export] public Array<Vector3> vertices  = [];
  [Export] public Array<Vector3> normals   = [];
  [Export] public Array<int>     triangles = [];

  [Export] public Material material;
  [Export] public Mesh.PrimitiveType type = Mesh.PrimitiveType.Triangles;

  /// <summary>
  /// Attach the surface to a mesh. The target mesh must be an ArrayMesh for this to work.
  /// </summary>
  public void AttachToMesh(ArrayMesh mesh)
  {
    if (!isValid()) return;

    Godot.Collections.Array arrays = [];
    arrays.Resize(13);
    if (vertices.Count > 0) arrays[(int)Mesh.ArrayType.Vertex] = vertices;
    if (normals.Count > 0)  arrays[(int)Mesh.ArrayType.Normal] = normals;
    if (triangles.Count > 0)arrays[(int)Mesh.ArrayType.Index]  = triangles;

    mesh.AddSurfaceFromArrays(type, arrays);
    if (material is not null) mesh.SurfaceSetMaterial(mesh.GetSurfaceCount() - 1, material);
  }

  public bool isValid()
  {
    return (vertices.Count > 0 && triangles.Count % 3 == 0) || triangles.Count > 0;
  }
}
