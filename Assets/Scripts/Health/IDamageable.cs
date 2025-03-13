using UnityEngine;

public interface IDamageable
{
	public float CurrentHealth { get; }
	public float MaxHealth { get; }

	public void TakeDamage(float damage);
	public void Die();
}
