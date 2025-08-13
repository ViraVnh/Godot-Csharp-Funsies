using System.Collections.Generic;
using System.Collections;
using Godot;

namespace Veila.data;

/// <summary>
/// Create a cache for values mapped to 2D indexes within a contiguous memory zone (1d array).
/// <br/>
/// <b>WARNING:</b> This class doesn't do any index verifications to reduce latency.
/// If you're uncertain an index will exist, this can cause a crash.
/// </summary>
[Tool]
public class Contiguous2DCache<T>(int size, int offset = 0) : IEnumerable<T>
{

  private readonly T[] cache = new T[size * size];
  private readonly int size = size;
  private readonly int offset = offset;

  public T this[int x, int y]
  {
    get => cache[x + offset + (y + offset) * size];
    set => cache[x + offset + (y + offset) * size] = value;
  }

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  public IEnumerator<T> GetEnumerator()
  {
    for (int i = 0; i < cache.Length; i++)
    {
      if (cache[i] is not null) yield return cache[i];
    }
  }

}
