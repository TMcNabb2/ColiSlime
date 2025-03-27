using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireWeapon : Weapon
{
    [Header("Fire Weapon Settings")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private LayerMask enemyLayer;
    
    // Ability properties
    private bool hasFlameShot = true; // Base ability
    private bool hasFlameBurst = false;
    private bool hasMoltenCore = false;
    private bool hasMeteorDrop = false;
    
    // Pool tags for prefabs
    private const string FIREBALL_TAG = "Fireball";
    private const string METEOR_TAG = "Meteor";
    
    protected override void Awake()
    {
        base.Awake();
        
        // Ensure fireballPrefab and meteorPrefab are properly set up in the PoolManager
        // This is now handled by the PoolManager SerializedPrefabPool
    }
    
    protected override void ApplyAbilityEffects(WeaponAbility ability)
    {
        switch (ability.abilityName)
        {
            case "Flame Shot":
                hasFlameShot = true;
                break;
            case "Flame Burst":
                hasFlameBurst = true;
                break;
            case "Molten Core":
                hasMoltenCore = true;
                break;
            case "Meteor Drop":
                hasMeteorDrop = true;
                break;
            case "Hellscape":
                UnlockUltimate();
                break;
        }
    }
    
    protected override void TryAttack()
    {
        // Find the closest enemy
        Transform target = FindClosestEnemy();
        
        if (target != null)
        {
            // Fire at the target
            FireAtTarget(target);
            
            // Start cooldown
            currentCooldown = cooldown;
        }
    }
    
    private Transform FindClosestEnemy()
    {
        // Find all enemies in range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        
        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        
        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = hitCollider.transform;
            }
        }
        
        return closest;
    }
    
    private void FireAtTarget(Transform target)
    {
        if (firePoint == null)
        {
            firePoint = transform;
        }
        
        // Calculate direction to target
        Vector3 direction = (target.position - firePoint.position).normalized;
        
        // Determine which projectile type to use
        GameObject prefabToUse = hasMeteorDrop ? meteorPrefab : fireballPrefab;
        string poolTag = hasMeteorDrop ? METEOR_TAG : FIREBALL_TAG;
        
        // How many projectiles to fire
        int projectileCount = hasMoltenCore ? 2 : 1;
        
        for (int i = 0; i < projectileCount; i++)
        {
            // Use PoolManager to get a fireball from the pool
            GameObject fireball = null;
            
            // Check if the pool exists in PoolManager
            if (PoolManager.Instance.Pools.ContainsKey(prefabToUse.name))
            {
                fireball = PoolManager.Instance.Pools[prefabToUse.name].Get();
                fireball.transform.position = firePoint.position;
                fireball.transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                // Fallback to instantiation if pool doesn't exist
                fireball = Instantiate(prefabToUse, firePoint.position, Quaternion.LookRotation(direction));
                Debug.LogWarning($"Pool for {prefabToUse.name} not found in PoolManager. Creating new instance.");
            }
            
            if (fireball != null)
            {
                // Configure the fireball based on abilities
                Fireball fireballComponent = fireball.GetComponent<Fireball>();
                if (fireballComponent != null)
                {
                    fireballComponent.doubleDamage = hasMoltenCore;
                    fireballComponent.leaveBurningGround = hasFlameBurst;
                    fireballComponent.isMeteor = hasMeteorDrop;
                    fireballComponent.sizeMultiplier = hasFlameBurst ? 1.5f : 1f;
                    
                    // Fireball will call OnObjectSpawn if it implements IPooledObject
                }
                
                // Slight variation for multiple projectiles
                if (projectileCount > 1 && i > 0)
                {
                    fireball.transform.rotation = Quaternion.LookRotation(direction + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f)));
                }
            }
        }
    }
    
    protected override void UseUltimateAbility()
    {
        // Hellscape - rain of meteors
        StartCoroutine(HellscapeUltimate());
    }
    
    private IEnumerator HellscapeUltimate()
    {
        // Duration of the ultimate
        float duration = 5f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            // Spawn meteors randomly around the player
            for (int i = 0; i < 3; i++) // Spawn 3 meteors each 0.2 seconds
            {
                // Random position around the player
                Vector3 randomPos = transform.position + new Vector3(Random.Range(-10f, 10f), 15f, Random.Range(-10f, 10f));
                
                // Direction toward the ground
                Vector3 direction = Vector3.down;
                
                // Use PoolManager to spawn a meteor
                GameObject meteor = null;
                
                if (PoolManager.Instance.Pools.ContainsKey(meteorPrefab.name))
                {
                    meteor = PoolManager.Instance.Pools[meteorPrefab.name].Get();
                    meteor.transform.position = randomPos;
                    meteor.transform.rotation = Quaternion.LookRotation(direction);
                }
                else
                {
                    // Fallback to instantiation if pool doesn't exist
                    meteor = Instantiate(meteorPrefab, randomPos, Quaternion.LookRotation(direction));
                    Debug.LogWarning($"Pool for {meteorPrefab.name} not found in PoolManager. Creating new instance.");
                }
                
                if (meteor != null)
                {
                    Fireball meteorComponent = meteor.GetComponent<Fireball>();
                    if (meteorComponent != null)
                    {
                        meteorComponent.doubleDamage = true;
                        meteorComponent.leaveBurningGround = true;
                        meteorComponent.isMeteor = true;
                        meteorComponent.sizeMultiplier = 2f;
                    }
                }
            }
            
            // Wait a bit before the next batch
            yield return new WaitForSeconds(0.2f);
            elapsed += 0.2f;
        }
    }
} 