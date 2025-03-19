using UnityEngine;

namespace SpawnWaves
{
	/// <summary>
	/// <inheritdoc cref="EnemyData"/>
	/// </summary>
	[CreateAssetMenu(fileName = "EnemyDataAsset", menuName = "Scriptable Objects/EnemyDataAsset")]
	public sealed class EnemyDataAsset : ScriptableObject
	{
		[Tooltip("The enemy's prefab.")]
		[SerializeField, Prefab] private GameObject _enemyPrefab;
		[Tooltip("Whether this enemy should be treated as a boss or not.")]
		[SerializeField] private bool _isBoss = false;
		[Tooltip("The enemy's difficulty value.")]
		[SerializeField, Range(0,1)] private float _difficulty;
		
		/// <summary>
		/// <inheritdoc cref="EnemyData.EnemyPrefab"/> (Read only)
		/// </summary>
		public GameObject EnemyPrefab { get => _enemyPrefab; private set => _enemyPrefab = value; }
		/// <summary>
		/// The enemy's difficulty value. (Read only)
		/// </summary>
		public float Difficulty { get => _difficulty; private set => _difficulty = value; }
		/// <summary>
		/// Whether this enemy should be treated as a boss or not.
		/// </summary>
		public bool IsBoss { get => _isBoss; set => _isBoss = value; }
	}
}