using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace JDoddsNAIT.Stats
{
    /// <summary>
    /// Performs queries on a set of base stats to apply modifiers to those stats. Derive from this class to specify the stat container.
    /// </summary>
    /// <typeparam name="TStats">The type of the container class that holds properties that can be modified.</typeparam>
    public abstract class Stats<TStats> : MonoBehaviour where TStats : ScriptableObject, new()
    {
        private readonly Type _type = typeof(TStats);
        private readonly StatMediator _mediator = new();
        private readonly Dictionary<string, PropertyInfo> _properties = new();

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
            foreach (var property in _type.GetProperties())
            {
                _properties.Add(property.Name, property);
            }

            if (_baseStats == null)
            {
                _baseStats = new();
            }
        }

        /// <summary>
        /// <inheritdoc cref="StatMediator.AddModifier(StatModifier)"/>
        /// </summary>
        /// <param name="modifier"></param>
        public void AddModifier(StatModifier modifier) => Mediator.AddModifier(modifier);
        /// <summary>
        /// <inheritdoc cref="AddModifier(StatModifier)"/>
        /// </summary>
        /// <param name="modifiers"></param>
        public void AddModifier(params StatModifier[] modifiers) => Mediator.AddModifier(modifiers);

        /// <summary>
        /// <inheritdoc cref="StatMediator.RemoveModifier(StatModifier)"/>
        /// </summary>
        /// <param name="modifier"></param>
        public void RemoveModifier(StatModifier modifier) => Mediator.RemoveModifier(modifier);

        /// <summary>
        /// Retrieves the modified value of a property with a given <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The modified value.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public object GetStat(string name)
        {
            if (!_properties.TryGetValue(name, out var prop))
            {
                throw new ArgumentOutOfRangeException(nameof(name), $"No property found with the name {name}.");
            }

            var q = new Query(prop, prop.GetValue(_baseStats));
            Mediator.PerformQuery(this, q);
            return q.Value;
        }

        /// <summary>
        /// <inheritdoc cref="GetStat(string)"/>
        /// </summary>
        /// <remarks>
        /// Generic version of <seealso cref="GetStat(string)"/>.
        /// </remarks>
        /// <typeparam name="T">The type to cast the return value into.</typeparam>
        /// <param name="name"></param>
        /// <returns>The modified value, as type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public T GetStat<T>(string name)
        {
            if (!_properties.TryGetValue(name, out var prop))
            {
                throw new ArgumentOutOfRangeException(nameof(name), $"No property found with the name {name}.");
            }

            if (prop.PropertyType != typeof(T))
            {
                throw new ArgumentException($"Property \"{name}\" is not of type {typeof(T).Name}.");
            }

            return (T)GetStat(prop.Name);
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