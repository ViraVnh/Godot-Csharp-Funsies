using System.Collections.Generic;
using System.Collections;
using Godot;

namespace Veila.data;

/// <summary>
/// Same as Contiguous2DCache but with more features, but also a bit more heavy.
/// </summary>
[Tool]
public class Contiguous2DCacheFull<T>(int size, int offsetX, int offsetY) : IEnumerable<T>
{

  private readonly T[] cache = new T[size * size];
  private readonly int size = size;
  private readonly Vector2I offset = new(offsetX, offsetY);

  public T this[int x, int y]
  {
    get => cache[x + offset.X + (y + offset.Y) * size];
    set => cache[x + offset.X + (y + offset.Y) * size] = value;
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
