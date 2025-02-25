using UnityEngine;

namespace JDoddsNAIT.Stats.Modifiers
{
	public class Multiplier : StatModifier<float>
	{
		public float Factor { get; set; }

		public Multiplier(float multiplier, float duration = 0, params string[] names) : base(duration, names)
		{
			Factor = multiplier;
		}

		protected override void OnHandle(object sender, Query<float> query)
		{
			query.Value *= Factor;
		}
	}
}