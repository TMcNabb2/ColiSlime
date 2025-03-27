using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [Header("Weapon Settings")]
        [SerializeField] protected string weaponName = "Base Weapon";
        [SerializeField] protected float damage = 10f;
        [SerializeField] protected float fireRate = 1f; // Shots per second
        [SerializeField] protected float range = 10f;
        
        protected float timeSinceLastFire = 0f;
        protected bool canFire = true;
        
        protected virtual void Update()
        {
            if (!canFire)
            {
                timeSinceLastFire += Time.deltaTime;
                if (timeSinceLastFire >= 1f / fireRate)
                {
                    canFire = true;
                }
            }
            
            TryFindAndAttackTarget();
        }
        
        protected abstract void TryFindAndAttackTarget();
        protected abstract void Fire(Vector3 targetPosition);
    }
} 