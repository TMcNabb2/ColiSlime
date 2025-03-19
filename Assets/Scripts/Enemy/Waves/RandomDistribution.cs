using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

/// <summary>
/// Generic interface for items that can be used in a <see cref="RandomDistribution"/> of <typeparamref name="TValue"/>s.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IDistributable<TValue>
{
	/// <summary>
	/// The value to retrieve if this item is selected.
	/// </summary>
	/// <returns></returns>
	TValue GetValue();
	/// <summary>
	/// The weight of the item in the distribution. The higher this number, to more likely this item will be chosen.
	/// </summary>
	int Weight { get; }
}

/// <summary>
/// Represents a random distribution of <typeparamref name="TValue"/>s.
/// </summary>
/// <typeparam name="TValue">The value type of the distribution.</typeparam>
public class RandomDistribution<TValue>
{
	private IDistributable<TValue>[] _items;

	/// <summary>
	/// The total weight of all the items in this distribution. (Read only)
	/// </summary>
	public int Total { get; private set; }

	/// <summary>
	/// The array of items and their respective <see cref="IDistributable{T}.Weight"/>
	/// </summary>
	public IDistributable<TValue>[] Items {
		get => _items;
		set {
			_items = value.OrderBy(x => x.Weight).ToArray();
			for (int i = Total = 0; i < _items.Length; Total += _items[i++].Weight) ;
		}
	}

	/// <summary>
	/// <inheritdoc cref="GetValueAt(int)"/>
	/// </summary>
	/// <param name="idx"></param>
	/// <returns></returns>
	public TValue this[int idx] => GetValueAt(idx);

	/// <summary>
	/// Constructs a random distribution of type <typeparamref name="TValue"/>.
	/// </summary>
	/// <param name="distribution"></param>
	public RandomDistribution(IEnumerable<IDistributable<TValue>> distribution)
	{
		_items = distribution.Where(x => x.Weight > 0).OrderBy(x => x.Weight).ToArray();
		for (int i = Total = 0; i < _items.Length; Total += _items[i++].Weight) ;
	}

	/// <summary>
	/// Gets a random value in the distribution. Items with higher weights will be chosen more often.
	/// </summary>
	/// <returns></returns>
	public TValue GetRandomValue() => GetValueAt(Random.Range(0, Total + 1));

	/// <summary>
	/// Gets a given <paramref name="amount"/> of random elements from the distribution.
	/// </summary>
	/// <remarks>
	/// See also: <seealso cref="GetRandomValue"/>
	/// </remarks>
	/// <param name="amount"></param>
	/// <returns>A collection of random values from the distribution.</returns>
	public TValue[] GetRandomValues(int amount)
	{
		var values = new TValue[amount];
		for (int i = 0; i < amount; values[i++] = GetRandomValue()) ;
		return values;
	}

	/// <summary>
	/// Gets the value at the given <paramref name="index"/>. An item's index in the distribution is defined as all integers between 0 and that items weight, plus the sum of all weights lower than its own.
	/// </summary>
	/// <param name="index"></param>
	/// <returns>The value of the item with the given <paramref name="index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException"/>
	public TValue GetValueAt(int index)
	{
		if (index < 0 || index > Total)
		{
			throw new IndexOutOfRangeException();
		}

		TValue result = default;
		int level = 0;
		bool found = false;
		for (int i = 0; i < Items.Length && !found; level += Items[i++].Weight)
		{
			found = index <= level + Items[i].Weight;
			if (found)
			{
				result = Items[i].GetValue();
			}
		}
		return result;
	}
}

/// <summary>
/// Static helper class for <see cref="RandomDistribution{TValue}"/>
/// </summary>
public static class RandomDistribution
{
	/// <summary>
	/// Creates a <see cref="RandomDistribution{TValue}"/> with the given values.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="distribution"></param>
	/// <returns></returns>
	public static RandomDistribution<T> Create<T>(IEnumerable<IDistributable<T>> distribution) => new(distribution);
}