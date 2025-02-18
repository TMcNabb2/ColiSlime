using System;

namespace JDoddsNAIT.Stats.Modifiers
{
	public class FunctionalModifier<T> : StatModifier<T>
	{
		public Func<T, T> Operation { get; set; }

		public FunctionalModifier(Func<T, T> operation, float duration = 0, params string[] stats) : base(duration, stats)
		{
			Operation = operation;
		}

		protected override void OnHandle(object sender, Query<T> query)
		{
			query.Value = Operation(query.Value);
		}
	}
}