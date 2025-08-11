
using Godot;

namespace Veila.libs;

[Tool]
public static class Math
{
  /// <summary>
  /// Calculates an average over time without storing all previous samples.
  /// Only works for sid > 1.
  /// </summary>
  /// <param name="average">The current accumulated average</param>
  /// <param name="newValue">The value to add to the average calculation</param>
  /// <param name="sid">The "id" of the new value</param>
  /// <returns>The new average calculated</returns>
  public static float AccumulatedAverage(float average, float newValue, float sid)
    =>  average * (sid-1)/sid + newValue/sid;
}
