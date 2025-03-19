using UnityEngine;

public static class BoolExtensions
{
	/// <summary>
	/// </summary>
	/// <param name="value"></param>
	/// <returns>1 if <see langword="true"/>, and 0 if <see langword="false"/>.</returns>
	public static int ToInt(this bool value) => value ? 1 : 0;
	/// <summary>
	/// </summary>
	/// <param name="value"></param>
	/// <returns>1 if <see langword="true"/>, and -1 if <see langword="false"/>.</returns>
	public static int ToIntSigned(this bool value) => value ? 1 : -1;

	/// <summary>
	/// </summary>
	/// <param name="value"></param>
	/// <returns>1 if <see langword="true"/>, and 0 if <see langword="null"/> or <see langword="false"/>.</returns>
	public static int ToInt(this bool? value) => value is true ? 1 : 0;
	/// <summary>
	/// </summary>
	/// <param name="value"></param>
	/// <returns>1 if <see langword="true"/>, 0 if <see langword="null"/>, and -1 if <see langword="false"/></returns>
	public static int ToIntSigned(this bool? value) => value switch {
		true => 1,
		null => 0,
		false => -1,
	};

	/// <summary>
	/// </summary>
	/// <param name="value"></param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is greater than 0, otherwise <see langword="false"/>.</returns>
	public static bool ToBool(this int value) => Mathf.Sign(value) > 0;
	/// <summary>
	/// </summary>
	/// <param name="value"></param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is greater than 0, <see langword="false"/> if less than 0, and <see langword="null"/> if equals 0.</returns>
	public static bool? ToBoolSigned(this int value) => ToBoolSigned((float)value);
	/// <summary>
	/// </summary>
	/// <param name="value"></param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is greater than 0, <see langword="false"/> if less than 0, and <see langword="null"/> if equals 0.</returns>
	public static bool? ToBoolSigned(this float value) => value switch {
		_ when value > 0 => true,
		_ when value < 0 => false,
		_ => null,
	};
}
