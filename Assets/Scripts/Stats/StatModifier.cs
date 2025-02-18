using System;

namespace JDoddsNAIT.Stats
{
	public interface IStatModifier : IDisposable
	{
		/// <summary>
		/// Handles an incoming <paramref name="query"/>.
		/// </summary>
		/// <param name="sender">The <see cref="Stats{TStats}"/> object that requested the query.</param>
		/// <param name="query">The <see cref="Query"/> object, holding the value to be modified.</param>
		public void Handle(object sender, Query query);
		/// <summary>
		/// This event is invoked as the modifier is being removed.
		/// </summary>
		public event Action<IStatModifier> OnDispose;
	}

	public abstract class StatModifier<T> : IStatModifier
	{
		/// <summary>
		/// Is true if the modifier is about to be removed.
		/// </summary>
		public bool MarkedForRemoval { get; private set; }

		public event Action<IStatModifier> OnDispose = delegate { };
		//public event Action<StatModifier<T>> OnDispose = delegate { };

		readonly CountdownTimer _timer;

		protected StatModifier(float duration)
		{
			if (duration <= 0) return;

			_timer = new(duration);
			_timer.OnStop += _ => Dispose();
			_timer.Start();
		}

		public void Handle(object sender, Query query)
		{
			if (query.Convert(out Query<T> q))
			{
				OnHandle(sender, q);
			}
		}

		/// <summary>
		/// Method determines how the incoming value will be modified.
		/// </summary>
		/// <param name="sender">The <see cref="Stats{TStats}"/> object that requested the query.</param>
		/// <param name="query">The <see cref="Query"/> object, holding the value to be modified.</param>
		protected abstract void OnHandle<TQuery>(object sender, Query<TQuery> query);

		public void Dispose()
		{
			MarkedForRemoval = true;
			_timer?.Dispose();
			OnDispose?.Invoke(this);
		}
	}
}