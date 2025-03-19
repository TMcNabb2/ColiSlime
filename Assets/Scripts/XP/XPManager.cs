using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class controls the current level and xp values. The level xp cap is increased by internal linear and exponential values. 
/// <br /> It also sends out events when the character levels up.
/// </summary>
public class XPManager : Singleton<XPManager>
{
	[Tooltip("Value is added after exponential increase.")]
	[SerializeField] private float _linearIncrease = 20f;

	[Tooltip("Value is counted as an increase in percentage so 80 = 1.8 times")]
	[SerializeField] private float _exponentialIncrease = 10f;

	[Space]

	private float _totalXPValue = 0;
	[SerializeField] private float _xp = 0;
	[SerializeField] private float _baseLevelXpCost = 20f;
	[SerializeField] private float _currentLevelXpCost;
	[SerializeField] private int _levels = 0;

	/// <summary>
	/// This event is called each time the xp the <see cref="XPManager"/> is tracking changes.
	/// </summary>
	public UnityEvent<float> XPValueChanged { get; private set; } = new UnityEvent<float>();

	/// <summary>
	/// If the level's value of xp to level up is changed this will be called with the new value.
	/// </summary>
	public UnityEvent<float> LevelValueChanged { get; private set; } = new UnityEvent<float>();

	/// <summary>
	/// Subscribe to this event to find when the player levels up.
	/// </summary>
	public UnityEvent LevelUp { get; private set; } = new UnityEvent();

	/// <summary>
	/// Add to this value to increase the xp held in the <see cref="XPManager"/>
	/// </summary>
	public float XP
	{
		get => _xp;
		set
		{
			if (_xp < value)
			{ // we do not want xp to ever go down, might change later.
				_totalXPValue += value - _xp;
				_xp = value;
				XPValueChanged.Invoke(_xp);
				if (_xp >= _currentLevelXpCost)
				{
					if (_levelRoutine != null)
					{
						StopCoroutine(_levelRoutine);
					}

					_levelRoutine = StartCoroutine(Leveling());
				}
			}
		}
	}

	/// <summary>
	/// Use this to get the number of levels that the <see cref="XPManager"/> is tracked.
	/// </summary>
	public int Level => _levels;

	private Coroutine _levelRoutine;

	protected override void Initialize()
	{
		ReCalculateLevel();
	}

	private IEnumerator Leveling()
	{
		while (_xp >= _currentLevelXpCost)
		{ // while the xp is larger than our cap
			if (Time.timeScale != 0)
			{ // not paused (in lvlup menu)
				_xp -= _currentLevelXpCost;
				CalculateNewLevelCost();
				_levels++;

				// call Unity Events
				LevelUp.Invoke();
				LevelValueChanged.Invoke(_currentLevelXpCost);
				XPValueChanged.Invoke(_xp);
			}
			yield return null; // I am having levelups happen one frame after another to allow levelup menus;
		}
	}

	private void CalculateNewLevelCost() => _currentLevelXpCost = _currentLevelXpCost * (1 + _exponentialIncrease * 0.01f) + _linearIncrease;

	/// <summary>
	/// Use this method if you require to calculate the current level cost from a new base level cost. This may cause a number of level ups if the base cost is lower than it started.
	/// </summary>
	public void ReCalculateLevel()
	{
		_xp = _totalXPValue;
		_currentLevelXpCost = _baseLevelXpCost;
		int levelBefore = _levels;
		_levels = 0;
		for (int i = 0; i < levelBefore && _xp >= _currentLevelXpCost; i++)
		{
			_xp -= _currentLevelXpCost;
			CalculateNewLevelCost();
			_levels++;
		}

		if (_levelRoutine != null)
		{
			StopCoroutine(_levelRoutine);
		}
		_levelRoutine = StartCoroutine(Leveling());
	}
}
