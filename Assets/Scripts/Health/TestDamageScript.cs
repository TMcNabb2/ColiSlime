using UnityEngine;

public class TestDamageScript : MonoBehaviour
{
	private IDamageable _damage;
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent(out _damage))
		{
			_damage.TakeDamage(5.3f);
			if (gameObject.TryGetComponent(out IDamageable d)) d.TakeDamage(3.2f);
		}
	}
}
