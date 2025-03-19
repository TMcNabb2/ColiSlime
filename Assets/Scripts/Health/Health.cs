using UnityEngine;
using Slider = UnityEngine.UI.Slider;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField, Min(0)] private float _health;
    [SerializeField, Min(0)] private float _maxHealth;
	[SerializeField] private Slider _healthSlider;
	[SerializeField] private GameObject destroyEffect;
	public float CurrentHealth => _health;
	public float MaxHealth => _maxHealth;

	private void Awake()
	{
		if (_healthSlider == null && !TryGetComponent(out _healthSlider))
		{
			Debug.LogError($"{name} could not find a <color='green'>Slider</color> on itself.");
		}
		_healthSlider.maxValue = _maxHealth;
		_healthSlider.value = _health;
	}
	public void TakeDamage(float damage)
	{
		_health = Mathf.Clamp(_health - damage, 0, _maxHealth);
		_healthSlider.value = _health;

		if (_health == 0) Die();
	}
	public void Die()
	{

        XPParticleTriggerHandler.Instance.EmitXP(10, transform.position);
        gameObject.SetActive(false);
		Instantiate(destroyEffect, transform.position, Quaternion.identity);
	}
}
