using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the pickup field's attraction range and trigger area.
/// Pulls in XP particles and collects them when they enter the trigger zone.
/// </summary>

[RequireComponent(typeof(ParticleSystemForceField))]
[RequireComponent(typeof(SphereCollider))]
public class PickupField : MonoBehaviour
{
	#region Fields

	[Header("Pickup Settings")]
	[SerializeField] private float _pickupRadius = 5f;
	[SerializeField] private float _pullStrength = -1f;

	[Header("Trigger Settings")]
	[SerializeField] private float _triggerRadius = 3f;

	[Header("References")]
	[SerializeField] private ParticleSystemForceField _forceField;
	[SerializeField] private SphereCollider _triggerCollider;
	[SerializeField] private XPParticleTriggerHandler _xpParticleHandler;

	#endregion

	#region Unity Methods

	private void Awake()
	{
		_forceField = GetComponent<ParticleSystemForceField>();
		_triggerCollider = GetComponent<SphereCollider>();

		if (_xpParticleHandler == null) Debug.LogError("XP_ParticleTriggerHandler is not assigned in PickupField!");

	}

	private void Start()
	{
		UpdatePickupSettings();
	}

	#endregion

	#region Helper Methods

	/// <summary>
	/// Updates pickup radius and force field settings.
	/// </summary>
	private void UpdatePickupSettings()
	{
		if (_forceField == null || _triggerCollider == null) return;

		_forceField.endRange = _pickupRadius;
		_forceField.gravityFocus = -_pullStrength;
		_triggerCollider.radius = _triggerRadius;
	}

	#endregion
}
