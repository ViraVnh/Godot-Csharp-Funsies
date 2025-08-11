/**
 * STATUS: WIP
 * Mostly work, but misses a lot of features.
 */

using System.Collections.Generic;
using Godot;

namespace Veila.classes.mesh;

/// <summary>
/// Handles a mesh surface as its own resource.
/// Handy in procedural mesh generation.
/// 
/// /!\ For performance this has been decoupled from Godot's native arrays to use
/// C# Lists instead. If you want a resource compatible see SurfaceResource.
/// </summary>
[Tool]
public class Surface
{
  public List<Vector3> vertices  = [];
  public List<Vector3> normals   = [];
  public List<int>     triangles = [];

  public Material material;
  public Mesh.PrimitiveType type = Mesh.PrimitiveType.Triangles;
  
  public Surface() {}
  public Surface(int verticesCount, int trianglesCount)
  {
    vertices  = new(verticesCount);
    normals   = new(verticesCount);
    triangles = new(trianglesCount);
  }

  /// <summary>
  /// Attach the surface to a mesh. The target mesh must be an ArrayMesh for this to work.
  /// </summary>
  public void AttachToMesh(ArrayMesh mesh)
  {
    if (!isValid()) return;

    Godot.Collections.Array arrays = [];
    arrays.Resize(13);
    if (vertices.Count > 0) arrays[(int)Mesh.ArrayType.Vertex] = vertices .ToArray();
    if (normals.Count > 0)  arrays[(int)Mesh.ArrayType.Normal] = normals  .ToArray();
    if (triangles.Count > 0)arrays[(int)Mesh.ArrayType.Index]  = triangles.ToArray();

    mesh.AddSurfaceFromArrays(type, arrays);
    if (material is not null) mesh.SurfaceSetMaterial(mesh.GetSurfaceCount() - 1, material);
  }

  public bool isValid()
  {
    return (vertices.Count > 0 && triangles.Count % 3 == 0) || triangles.Count > 0;
  }
}
