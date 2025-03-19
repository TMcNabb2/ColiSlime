using System;
using UnityEngine;

namespace SpawnWaves
{
	/// <summary>
	/// Container class for enemy data.
	/// </summary>
	[Serializable]
	public class EnemyData : IDistributable<GameObject>
	{
		[Tooltip("The enemy's prefab.")]
		[SerializeField, Prefab] private GameObject _enemyPrefab;
		[Tooltip("Whether this enemy should be treated as a boss or not.")]
		[SerializeField] private bool _isBoss = false;
		[Tooltip("The enemy's weight, or relative chance to be chosen to spawn in a wave.")]
		[SerializeField, Min(0)] private int _distributionWeight;

		/// <summary>
		/// The enemy's prefab.
		/// </summary>
		public GameObject EnemyPrefab { get => _enemyPrefab; set => _enemyPrefab = value; }
		/// <summary>
		/// The enemy's weight, or relative chance to be chosen in a <see cref="RandomDistribution"/>.
		/// </summary>
		public int Weight { get => _distributionWeight; set => _distributionWeight = value; }
		/// <summary>
		/// Whether this enemy should be treated as a boss or not.
		/// </summary>
		public bool IsBoss { get => _isBoss; set => _isBoss = value; }

		public EnemyData(GameObject enemyPrefab, int distributionWeight, bool isBoss = false)
		{
			_enemyPrefab = enemyPrefab;
			_distributionWeight = distributionWeight;
			_isBoss = isBoss;
		}

		public GameObject GetValue() => EnemyPrefab;
	}
}