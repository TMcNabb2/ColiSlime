using System;
using UnityEngine;

namespace JDoddsNAIT.Stats
{
	public interface IStatContainer
	{
		Query GetBaseValue(string statName);
	}

	/// <summary>
	/// Performs queries on a set of base stats to apply modifiers to those stats. Derive from this class to specify the stat container.
	/// </summary>
	/// <typeparam name="TStats">The type of the container class that holds properties that can be modified.</typeparam>
	public abstract class Stats<TStats> : MonoBehaviour where TStats : ScriptableObject, IStatContainer, new()
	{
		//private readonly Type _type = typeof(TStats);
		private readonly StatMediator _mediator = new();

		/// <summary>
		/// The stat mediator in change of adding and removing modifiers. (Read only)
		/// </summary>
		public StatMediator Mediator => _mediator;

		[SerializeField] private TStats _baseStats;
		/// <summary>
		/// The base stats that will be modified. (Read only)
		/// </summary>
		public TStats BaseStats => _baseStats;

		/// <summary>
		/// Call base.Awake() if overriding this method.
		/// </summary>
		protected virtual void Awake()
		{
			if (_baseStats == null)
			{
				_baseStats = new();
			}
		}

		/// <summary>
		/// <inheritdoc cref="StatMediator.AddModifier(IStatModifier)"/>
		/// </summary>
		/// <param name="modifier"></param>
		public void AddModifier(IStatModifier modifier) => Mediator.AddModifier(modifier);
		/// <summary>
		/// <inheritdoc cref="AddModifier(IStatModifier)"/>
		/// </summary>
		/// <param name="modifiers"></param>
		public void AddModifier(params IStatModifier[] modifiers) => Mediator.AddModifier(modifiers);

		/// <summary>
		/// <inheritdoc cref="StatMediator.RemoveModifier(IStatModifier)"/>
		/// </summary>
		/// <param name="modifier"></param>
		public void RemoveModifier(IStatModifier modifier) => Mediator.RemoveModifier(modifier);

		/// <summary>
		/// Retrieves the modified value of a property with a given <paramref name="name"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns>The modified value.</returns>
		/// <exception cref="Exception"></exception>
		public T GetStat<T>(string name)
		{
			var q = BaseStats.GetBaseValue(name);
			if (!q.Convert(out Query<T> query))
			{
				throw new Exception("Cannot get stat; query type is invalid.");
			}

			Mediator.PerformQuery(this, query);
			return query.Value;
		}

		/// <summary>
		/// Call base.OnDestroy() if overriding this method.
		/// </summary>
		protected virtual void OnDestroy()
		{
			Mediator.Dispose();
		}
	}
}