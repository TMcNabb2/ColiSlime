using UnityEngine;

public static class VectorExtensions
{
	/// <summary>
	/// <inheritdoc cref="RandomValueOnDonut(RangeFloat)"/>
	/// </summary>
	/// <param name="minValue"></param>
	/// <param name="maxValue"></param>
	/// <returns></returns>
	public static Vector2 RandomValueOnDonut(float minValue, float maxValue)
		=> RandomValueOnDonut(new RangeFloat(minValue, maxValue));

	/// <summary>
	/// Gets a random value on a circle but ensures the radius is variable between two distances
	/// </summary>
	/// <param name="range"></param>
	/// <returns></returns>
	public static Vector2 RandomValueOnDonut(RangeFloat range)
	{
		float radius = range.Lerp(Random.value);
		return Random.insideUnitCircle.normalized * radius;
	}

	public static Vector3 FlatV3(this Vector2 value) => new(value.x, 0, value.y);
	public static Vector3 Flat(this Vector3 value) => value.With(y: 0);

	public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
	{
		return new Vector2(x ?? vector.x, y ?? vector.y);
	}

	public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
	{
		return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
	}

	public static Vector2 Add(this Vector2 vector, float? x = null, float? y = null)
	{
		vector.x += x ?? 0;
		vector.y += y ?? 0;
		return vector;
	}

	public static Vector3 Add(this Vector3 vector, float? x = null, float?y = null, float? z = null)
	{
		vector.x += x ?? 0;
		vector.y += y ?? 0;
		vector.z += z ?? 0;
		return vector;
	}
}
