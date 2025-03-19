using UnityEngine;
using UnityEngine.UIElements;

public class UIElementsHealth : MonoBehaviour, IDamageable
{
	[SerializeField, Min(0)] private float _health;
	[SerializeField, Min(0)] private float _maxHealth;
	[SerializeField] private string _healthBarName;
	private ProgressBar _healthBar;

	public float CurrentHealth => _health;
	public float MaxHealth => _maxHealth;

	private void Awake()
	{
		var uiDocument = FindFirstObjectByType<UIDocument>();
		var root = uiDocument.rootVisualElement;
		_healthBar = root.Q<ProgressBar>(_healthBarName);
		if (_healthBar == null)
		{
			Debug.LogError($"Could not find a <color=#00FF00>ProgressBar</color> with name '{_healthBarName}'.");
			return;
		}

		_healthBar.highValue = _maxHealth;
		_healthBar.lowValue = 0;
		_healthBar.value = _health;
	}
    private void Update()
    {
        _healthBar.title = $"HP: {_health}/{_maxHealth}";
    }
    public void TakeDamage(float damage)
	{
		_health = Mathf.Clamp(_health - damage, 0, _maxHealth);
		_healthBar.value = _health;
		if (_health == 0) Die();
	}
	public void Die()
	{
		gameObject.SetActive(false);
	}
}
