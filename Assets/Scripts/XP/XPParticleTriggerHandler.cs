using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Particle = UnityEngine.ParticleSystem.Particle;
/// <summary>
/// Handles particle system triggers for XP collection.
/// Assigns XP values to particles and detects when they enter the trigger.
/// </summary>

[RequireComponent(typeof(ParticleSystem))]
public class XPParticleTriggerHandler : Singleton<XPParticleTriggerHandler>
{
	private ParticleSystem _xpParticleSystem;
	private List<Particle> _triggeredParticles = new();
	private ParticleSystem.EmitParams _emitParams = new() { startLifetime = Mathf.Infinity };
	private Particle[] _activeParticles;
	private List<float> _particleXPValues;
	
	protected override void Initialize()
	{
		_xpParticleSystem = GetComponent<ParticleSystem>();
		_particleXPValues = new List<float>(_xpParticleSystem.main.maxParticles);
		_activeParticles = new Particle[_xpParticleSystem.main.maxParticles];
	}


	/// <summary>
	/// Emits XP particles and assigns them values.
	/// Call this whenever XP should be spawned.
	/// Ensures particles don't die out
	/// </summary>
	public void EmitXP(float xpValue, Vector3 position)
	{
		int count = _xpParticleSystem.particleCount;
		if (count < _xpParticleSystem.main.maxParticles)
		{
			_particleXPValues.Add(xpValue);
			_emitParams.position = position;
			_emitParams.startColor = Color.red;
			_xpParticleSystem.Emit(_emitParams, 1);
		}
		else
		{
			_particleXPValues[^1] += xpValue;
		}
	}

	private void OnParticleTrigger()
	{
		if (_xpParticleSystem == null) return;

		int count = _xpParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _triggeredParticles);
		int activeCount = _xpParticleSystem.GetParticles(_activeParticles);

		float totalXP = 0;
		List<int> removedXPValues = new List<int>(activeCount);
		for (int i = 0; i < count; i++)
		{ // foreach triggeredParticle
			bool found = false;
			for (int j = 0; j < activeCount && !found; j++)
			{ // foreach activeParticle
				if (_triggeredParticles[i].position == _activeParticles[j].position)
				{ // add xp to total
					found = true;
					totalXP += _particleXPValues[j];
					removedXPValues.Add(j); // add the xp value to be removed in the list.
				}
			}
		}

		int[] sortedArray = removedXPValues.ToArray();
		Array.Sort(sortedArray);
		for (int i = sortedArray.Length - 1; i >= 0; i--)
		{ _particleXPValues.RemoveAt(sortedArray[i]); }

		XPManager.Instance.XP += totalXP;
	}
}
