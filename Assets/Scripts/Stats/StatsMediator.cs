using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace JDoddsNAIT.Stats
{
    public class StatMediator : IDisposable
    {
        readonly LinkedList<StatModifier> _modifiers = new();

        public event EventHandler<Query> Queries;
        public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

        /// <summary>
        /// Adds a <paramref name="modifier"/> to the mediator.
        /// </summary>
        /// <param name="modifier">The <see cref="StatModifier"/> to add.</param>
        public void AddModifier(StatModifier modifier)
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
        /// <inheritdoc cref="AddModifier(StatModifier)"/>
        /// </summary>
        /// <param name="modifiers"></param>
        public void AddModifier(params StatModifier[] modifiers)
        {
            foreach (StatModifier modifier in modifiers)
            {
                AddModifier(modifier);
            }
        }

        /// <summary>
        /// Removes a <paramref name="modifier"/> from the mediator.
        /// </summary>
        /// <param name="modifier">The <see cref="StatModifier"/> to remove.</param>
        public void RemoveModifier(StatModifier modifier)
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
        private readonly PropertyInfo _property;
        /// <summary>
        /// Name of the property being modified.
        /// </summary>
        public string PropertyName => _property.Name;
        /// <summary>
        /// the value currently being modified.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Creates a <see cref="Query"/> with the given <paramref name="propertyInfo"/> and initial <paramref name="value"/>.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="value">The property's initial or base value.</param>
        public Query(PropertyInfo propertyInfo, object value)
        {
            _property = propertyInfo;
            Value = value;
        }
    }
}
