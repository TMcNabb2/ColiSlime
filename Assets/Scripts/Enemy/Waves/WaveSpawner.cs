using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpawnWaves
{
	/// <summary>
	/// Component for spawning waves.
	/// </summary>
	public class WaveSpawner : MonoBehaviour
	{
		[SerializeField] private RangeFloat _spawnRadius = new(10, 20);

		private static Queue<SpawnWave> _activeWaves;
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Init() => _activeWaves = new();

		/// <summary>
		/// Starts spawning a given <paramref name="wave"/>
		/// </summary>
		/// <param name="wave"></param>
		public void SpawnWave(SpawnWave wave)
		{
			wave.OnSpawn += this.OnSpawn;
			wave.OnWaveEnd += this.OnWaveEnd;
			wave.Start();
		}

		/// <summary>
		/// Starts spawning the next wave in the queue
		/// </summary>
		public void SpawnNextWave()
		{
			if (_activeWaves.TryDequeue(out var wave))
			{
				wave.OnSpawn += OnSpawn;
				wave.OnWaveEnd += OnWaveEnd;
				wave.Start();
			}
		}

		/// <summary>
		/// Adds wave to the queue so it may be spawned in the future.
		/// </summary>
		/// <param name="wave"></param>
		public void AddWaveToQueue(SpawnWave wave) => _activeWaves.Enqueue(wave);
		
		/// <summary>
		/// Immediately stops a wave from spawning.
		/// </summary>
		/// <param name="wave"></param>
		public void StopWave(SpawnWave wave)
		{
			if (_activeWaves.Contains(wave))
			{
				_activeWaves = new Queue<SpawnWave>(_activeWaves.Where(x => x != wave));
				wave.Dispose();
			}
		}
		
		/// <summary>
		/// Immediately stops all waves from spawning.
		/// </summary>
		/// <param name="wave"></param>
		public void ClearWaves()
		{
			for (int i = 0; i < _activeWaves.Count; i++)
				_activeWaves.Dequeue().Dispose();
		}

		private void OnWaveEnd(SpawnWave wave, Timers.ITimer timer)
		{
			wave.Dispose();
			SpawnNextWave();
		}

		private void OnSpawn(GameObject prefab, Timers.ITimer timer)
		{
			var realObj = PoolManager.Instance.Pools[prefab.name].Get();
			if (realObj == null) return;
			realObj.transform.position = VectorExtensions.RandomValueOnDonut(_spawnRadius).FlatV3();
			realObj.transform.position += Camera.main.transform.position.Flat();
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(Camera.main.transform.position.Flat(), _spawnRadius.Min);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(Camera.main.transform.position.Flat(), _spawnRadius.Max);
		}
	}
}