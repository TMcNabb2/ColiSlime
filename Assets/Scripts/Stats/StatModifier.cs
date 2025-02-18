using System;
using UnityEditor.Search;
using UnityEngine;

namespace JDoddsNAIT.Stats
{
    public abstract class StatModifier : IDisposable
    {
        /// <summary>
        /// Is true if the modifier is about to be removed.
        /// </summary>
        public bool MarkedForRemoval { get; private set; }

        /// <summary>
        /// This event is invoked as the modifier is being removed.
        /// </summary>
        public event Action<StatModifier> OnDispose = delegate { };

        readonly CountdownTimer _timer;

        protected StatModifier(float duration)
        {
            if (duration <= 0) return;

            _timer = new(duration);
            _timer.OnStop += _ => Dispose();
            _timer.Start();
        }

        /// <summary>
        /// Handles an incoming <paramref name="query"/>.
        /// </summary>
        /// <param name="sender">The <see cref="Stats{TStats}"/> object that requested the query.</param>
        /// <param name="query">The <see cref="Query"/> object, holding the value to be modified.</param>
        public void Handle(object sender, Query query)
        {
            OnHandle(sender, query);
        }

        /// <summary>
        /// Method determines how the incoming value will be modified.
        /// </summary>
        /// <param name="sender">The <see cref="Stats{TStats}"/> object that requested the query.</param>
        /// <param name="query">The <see cref="Query"/> object, holding the value to be modified.</param>
        protected abstract void OnHandle(object sender, Query query);

        public void Dispose()
        {
            MarkedForRemoval = true;
            _timer?.Dispose();
            OnDispose?.Invoke(this);
        }
    }

    /// <summary>
    /// Basic modifier that performs an operation on the value.
    /// </summary>
    /// <typeparam name="T">The values type.</typeparam>
    public class BasicStatModifier<T> : StatModifier
    {
        private readonly string _name;
        protected readonly Func<T, T> _operation;

        public BasicStatModifier(string name, Func<T, T> operation, float duration = 0) : base(duration)
        {
            _name = name;
            _operation = operation;
        }

        protected override void OnHandle(object sender, Query query)
        {
            if (query.PropertyName == _name && query.Value is T value)
            {
                query.Value = _operation(value);
            }
        }
    }
}