using System;
using System.Collections.Generic;

namespace JDoddsNAIT.Stats
{
	public class StatMediator : IDisposable
	{
		readonly LinkedList<IStatModifier> _modifiers = new();

		public event EventHandler<Query> Queries;
		public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

		/// <summary>
		/// Adds a <paramref name="modifier"/> to the mediator.
		/// </summary>
		/// <param name="modifier">The <see cref="IStatModifier"/> to add.</param>
		public void AddModifier(IStatModifier modifier)
		{
			_modifiers.AddLast(modifier);
			Queries += modifier.Handle;

			modifier.OnDispose += _ =>
			{
				_modifiers.Remove(modifier);
				Queries -= modifier.Handle;
			};
		}

		/// <summary>
		/// <inheritdoc cref="AddModifier(IStatModifier)"/>
		/// </summary>
		/// <param name="modifiers"></param>
		public void AddModifier(params IStatModifier[] modifiers)
		{
			foreach (IStatModifier modifier in modifiers)
			{
				AddModifier(modifier);
			}
		}

		/// <summary>
		/// Removes a <paramref name="modifier"/> from the mediator.
		/// </summary>
		/// <param name="modifier">The <see cref="IStatModifier"/> to remove.</param>
		public void RemoveModifier(IStatModifier modifier)
		{
			if (_modifiers.Contains(modifier))
			{
				modifier.Dispose();
			}
		}

		public void Dispose()
		{
			var node = _modifiers.First;
			while (node != null)
			{
				var next = node.Next;
				node.Value.Dispose();
				node = next;
			}
		}
	}

	/// <summary>
	/// Container class for holding the name and value of the property being modified.
	/// </summary>
	public class Query
	{
		public string Name { get; set; }
		/// <summary>
		/// the value currently being modified.
		/// </summary>
		public object BoxedValue { get; set; }

		/// <summary>
		/// Creates a <see cref="Query"/> with the given <paramref name="statName"/> and initial <paramref name="value"/>.
		/// </summary>
		/// <param name="statName"></param>
		/// <param name="value">The property's initial or base value.</param>
		public Query(string statName, object value)
		{
			Name = statName;
			BoxedValue = value;
		}

		public bool Convert<T>(out Query<T> query)
		{
			query = null;
			if (BoxedValue is T value)
			{
				query = new Query<T>(Name, value);
			}
			return BoxedValue is T;
		}
	}

	public class Query<T>
	{
		public string Name { get; set; }
		public T Value { get; set; }

		public Query(string statName, T value)
		{
			Name = statName;
			Value = value;
		}
		public static implicit operator Query(Query<T> value) => new(value.Name, value.Value);
	}
}
