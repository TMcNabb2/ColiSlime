using System;
using System.Collections.Generic;
using System.Linq;
using Timers;
using UnityEngine;

namespace SpawnWaves
{
	/// <summary>
	/// Represents a wave of enemies spawned over time.
	/// </summary>
	[Serializable]
	public class SpawnWave : IDisposable
	{
		private static int _currentId;

		private int _currentIndex = 0;

		private readonly CountdownTimer _spawnTimer, _waveTimer;

		[SerializeField] private GameObject[] _enemyPrefabs;
		[SerializeField] private float _duration;

		/// <summary>
		/// The wave's ID.
		/// </summary>
		public readonly int Id;

		/// <summary>
		/// The enemies in the <see cref="SpawnWave"/>.
		/// </summary>
		public GameObject[] Enemies => _enemyPrefabs;
		/// <summary>
		/// The total duration of the wave.
		/// </summary>
		public float TotalTime => _duration;

		/// <summary>
		/// Event is invoked when an enemy spawns.
		/// </summary>
		public event Action<GameObject, ITimer> OnSpawn = delegate { };
		/// <summary>
		/// Event is invoked when the wave ends.
		/// </summary>
		public event Action<SpawnWave, ITimer> OnWaveEnd = delegate { };

		private SpawnWave(float totalTime, IEnumerable<GameObject> prefabs)
		{
			Id = _currentId++;

			_enemyPrefabs = prefabs.ToArray();

			_duration = totalTime;

			_spawnTimer = new(_duration / _enemyPrefabs.Length, repeat: true);
			_spawnTimer.OnStop += this.InvokeSpawn;
			_waveTimer = new(_duration);
			_waveTimer.OnStop += this.InvokeEnd;
		}

		private void InvokeSpawn(ITimer timer) => OnSpawn?.Invoke(_enemyPrefabs[_currentIndex++], timer);
		private void InvokeEnd(ITimer timer) => OnWaveEnd?.Invoke(this, timer);

		/// <summary>
		/// Start this wave's timers.
		/// </summary>
		public void Start()
		{
			_spawnTimer.Start();
			_waveTimer.Start();
		}

		/// <summary>
		/// Generates a <see cref="SpawnWave"/> using a collection of enemy data.
		/// </summary>
		/// <param name="enemies"></param>
		/// <param name="enemyCount"></param>
		/// <param name="totalTime"></param>
		/// <returns></returns>
		public static SpawnWave Generate(IList<EnemyData> enemies, int enemyCount, float totalTime)
		{
			List<GameObject> waveEnemies = new();
			foreach (EnemyData boss in enemies.Where(e => e.IsBoss))
			{
				waveEnemies.Add(boss.EnemyPrefab);
			}

			var dist = RandomDistribution.Create(enemies.Where(e => !e.IsBoss));

			if (dist.Items.Length == 0)
				return null;

			waveEnemies.AddRange(dist.GetRandomValues(enemyCount));

			return new SpawnWave(totalTime, waveEnemies);
		}

		public void Dispose()
		{
			OnSpawn = null;
			_spawnTimer?.Dispose();
			OnWaveEnd = null;
			_waveTimer?.Dispose();
		}

		public override int GetHashCode() => Id;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Init()
		{
			_currentId = 0;
		}
	}
}