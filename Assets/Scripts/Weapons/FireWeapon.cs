using UnityEngine;
using System.Collections.Generic;

namespace Weapons
{
    public class FireWeapon : Weapon
    {
        [Header("Fire Weapon Settings")]
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float explosionRadius = 2f;
        [SerializeField] private float burnDamage = 2f;
        [SerializeField] private float burnDuration = 3f;
        [SerializeField] private LayerMask enemyLayerMask; // Layer mask for enemies
        
        [Header("Upgrades")]
        [SerializeField] private bool flameBurstUnlocked = false;
        [SerializeField] private bool moltenCoreUnlocked = false;
        [SerializeField] private bool meteorDropUnlocked = false;
        [SerializeField] private float burnGroundDuration = 5f;
        
        private readonly float targetDetectionRadius = 15f;
        
        private void Awake()
        {
            weaponName = "Flame Shot";
            fireRate = 1f;
            damage = 15f;
            range = 20f;
            
            if (firePoint == null)
            {
                firePoint = transform;
            }
        }
        
        protected override void TryFindAndAttackTarget()
        {
            if (!canFire) return;
            
            // Find the closest enemy within detection radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, targetDetectionRadius, enemyLayerMask);
            GameObject closestEnemy = null;
            float closestDistance = float.MaxValue;
            
            foreach (var hitCollider in hitColliders)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider.gameObject;
                }
            }
            
            // If we found an enemy, fire at it
            if (closestEnemy != null)
            {
                Fire(closestEnemy.transform.position);
            }
        }
        
        protected override void Fire(Vector3 targetPosition)
        {
            canFire = false;
            timeSinceLastFire = 0f;
            
            // Direction to target
            Vector3 direction = (targetPosition - firePoint.position).normalized;
            
            // Create one or two fireballs based on upgrades
            int fireballCount = moltenCoreUnlocked ? 2 : 1;
            
            for (int i = 0; i < fireballCount; i++)
            {
                // Create the fireball
                GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.LookRotation(direction));
                
                // Make sure the fireball is not a child of the weapon
                fireball.transform.parent = null;
                
                FireballProjectile projectile = fireball.GetComponent<FireballProjectile>();
                
                if (projectile != null)
                {
                    // Configure projectile
                    projectile.Initialize(
                        damage,
                        direction,
                        explosionRadius,
                        burnDamage,
                        burnDuration,
                        flameBurstUnlocked,
                        meteorDropUnlocked,
                        burnGroundDuration,
                        transform.root.gameObject  // Player reference for knockback direction
                    );
                }
                else
                {
                    Debug.LogError("FireballPrefab does not have a FireballProjectile component!");
                }
                
                // For multiple fireballs, add a slight spread
                if (fireballCount > 1 && i < fireballCount - 1)
                {
                    direction = Quaternion.Euler(0, Random.Range(-15f, 15f), 0) * direction;
                }
            }
        }
        
        public void UnlockFlameBurst()
        {
            flameBurstUnlocked = true;
            explosionRadius *= 1.5f;
        }
        
        public void UnlockMoltenCore()
        {
            moltenCoreUnlocked = true;
            burnDamage *= 2f;
        }
        
        public void UnlockMeteorDrop()
        {
            meteorDropUnlocked = true;
        }
    }
}