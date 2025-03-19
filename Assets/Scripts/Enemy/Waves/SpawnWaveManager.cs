using System.Collections.Generic;
using UnityEngine;

namespace SpawnWaves
{
	public class SpawnWaveManager : Singleton<SpawnWaveManager>
	{
		[SerializeField] private WaveSpawner _spawner;
		[Space]
		[SerializeField] private EnemyDataAsset[] _enemyData;
		[Space]
		#region Wave Data
		[Tooltip("The current difficulty level.")]
		[SerializeField, Range(0, 1)] private float _difficultyFactor = 0;
		[Tooltip("The allowed deviation of an enemy's difficulty value from the difficulty factor.")]
		[SerializeField, Range(0, 1)] private float _difficultyRange = 0;
		[Tooltip("The width of the difficulty curve.")]
		[SerializeField, Min(0.01f)] private float _difficultyCurveWidth = 1;
		[Space]
		[Tooltip("The max value of an enemy's weight in a generated wave.")]
		[SerializeField, Min(1)] private int _precision = 100;
		[Tooltip("The amount of enemies per wave. The actual amount is an interpolation between the min and max by the difficulty factor.")]
		[SerializeField] private RangeInt _enemiesPerWave;
		[Tooltip("The duration of each generated wave. The actual amount is an interpolation between the min and max by the difficulty factor.")]
		[SerializeField] private RangeFloat _waveDuration;
		#endregion

		/// <summary>
		/// The current difficulty level. Average difficulty value of all enemies in a generated wave.
		/// </summary>
		/// <remarks>
		/// See also: <seealso cref="GetWeight(float, float, float, float)"/>
		/// </remarks>
		public float DifficultyFactor { get => _difficultyFactor; set => _difficultyFactor = Mathf.Clamp01(value); }
		/// <summary>
		/// The allowed deviation of an enemy's difficulty value and the <see cref="DifficultyFactor"/>. Determines what enemies can be spawned in a generated wave.
		/// </summary>
		/// <remarks>
		/// See also: <seealso cref="GetWeight(float, float, float, float)"/>
		/// </remarks>
		public float DifficultyRange { get => _difficultyRange; set => _difficultyRange = Mathf.Clamp01(value); }
		/// <summary>
		/// The width of the difficulty curve.
		/// </summary>
		/// <remarks>
		/// See also: <seealso cref="GetWeight(float, float, float, float)"/>
		/// </remarks>
		public float DifficultyCurveWidth { get => _difficultyCurveWidth; set => _difficultyCurveWidth = Mathf.Clamp(value, 0.01f, float.MaxValue); }

		private static float Sqr(float n) => n * n;

		/// <summary>
		/// This function returns the normalized distribution weight of an enemy with a difficulty of <paramref name="x"/>, where <paramref name="t"/> is the difficulty factor, <paramref name="w"/> is the width of the curve, and <paramref name="d"/> is the allowed deviation. 
		/// </summary>
		/// <param name="t">The difficulty factor, or horizontal displacement of the curve.</param>
		/// <param name="w">The width of the curve.</param>
		/// <param name="x">The enemy's difficulty value.</param>
		/// <param name="d">The allowed range of difficulties.</param>
		/// <returns>1 - ((<paramref name="x"/> - <paramref name="t"/>) / <paramref name="w"/>)^2; [<paramref name="t"/> - <paramref name="d"/>/2, <paramref name="t"/> + <paramref name="d"/>/2]</returns>
		public static float GetWeight(float x, float t, float w, float d) => Mathf.Clamp01(1 - Sqr((x - t) / w)) * 
			// Limit the result of the function.
			(t - (d / 2f), t + (d / 2f)).Contains(x).ToInt();

		protected override void Initialize()
		{
			Debug.Assert(_spawner != null);
		}

		/// <summary>
		/// Generates and spawns a wave.
		/// </summary>
		[ContextMenu("Generate Wave")]
		public SpawnWave GenerateWave()
		{
			var waveEnemies = new List<EnemyData>();
			foreach (var enemy in _enemyData)
			{
				waveEnemies.Add(new EnemyData(
					enemyPrefab: enemy.EnemyPrefab,
					distributionWeight: (int)(_precision * GetWeight(
						x: enemy.Difficulty,
						t: _difficultyFactor,
						w: _difficultyCurveWidth,
						d: _difficultyRange))));
			}

			return SpawnWave.Generate(
				enemies: waveEnemies,
				enemyCount: _enemiesPerWave.Lerp(_difficultyFactor),
				totalTime: _waveDuration.Lerp(_difficultyFactor));
		}

		/// <summary>
		/// Generates an array of <see cref="SpawnWave"/>s.
		/// </summary>
		/// <param name="count"></param>
		/// <param name="difficultyRange"></param>
		/// <returns></returns>
		public SpawnWave[] GenerateWaves(int count, RangeFloat difficultyRange)
		{
			float increment = (difficultyRange.Max - difficultyRange.Min) / count;
			var result = new SpawnWave[count];
			for (int i = 0; i < count; i++)
			{
				_difficultyFactor = difficultyRange.Lerp(i * increment);
				result[i] = GenerateWave();
				if (result[i] == null) Debug.LogWarning($"Wave #{i + 1} failed to generate! :D");
				else
				{
					_spawner.AddWaveToQueue(result[i]);
				}
			}
			return result;
		}
		public void StartNextWave()
		{
			_spawner.SpawnNextWave();
		}
	}
}