using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{
    // Projectile properties
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float burnDuration = 3f;
    [SerializeField] private float burnDamagePerSecond = 2f;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject fireTrailPrefab;
    
    // Ability modifiers
    [HideInInspector] public bool doubleDamage = false;
    [HideInInspector] public bool leaveBurningGround = false;
    [HideInInspector] public bool isMeteor = false;
    [HideInInspector] public float sizeMultiplier = 1f;
    
    private Rigidbody rb;
    private float currentLifetime;
    private GameObject activeTrail;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        // Configure Rigidbody
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        // Add a sphere collider if it doesn't exist
        SphereCollider col = GetComponent<SphereCollider>();
        if (col == null)
        {
            col = gameObject.AddComponent<SphereCollider>();
            col.radius = 0.5f;
            col.isTrigger = true;
        }
    }
    
    // OnEnable is called when the object is activated (from pool)
    private void OnEnable()
    {
        // Reset lifetime
        currentLifetime = lifetime;
        
        // Scale based on size multiplier
        transform.localScale = Vector3.one * sizeMultiplier;
        
        // Apply meteor physics if needed
        if (isMeteor)
        {
            rb.useGravity = true;
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
        else
        {
            rb.useGravity = false;
        }
        
        // Create fire trail effect
        if (fireTrailPrefab != null && activeTrail == null)
        {
            activeTrail = Instantiate(fireTrailPrefab, transform.position, Quaternion.identity, transform);
        }
    }
    
    private void Update()
    {
        // Count down lifetime
        currentLifetime -= Time.deltaTime;
        if (currentLifetime <= 0)
        {
            Explode();
        }
    }
    
    private void FixedUpdate()
    {
        // For meteor mode, we let gravity do the work
        if (!isMeteor)
        {
            // Move the fireball forward
            rb.linearVelocity = transform.forward * speed;
        }
        else
        {
            // For meteor, add some forward movement but let gravity pull it down
            rb.AddForce(transform.forward * speed * 0.1f, ForceMode.Force);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if we hit an enemy or the ground
        if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {
            Explode();
        }
    }
    
    private void Explode()
    {
        // Apply damage to objects in explosion radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Apply damage
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    float finalDamage = damage * (doubleDamage ? 2f : 1f);
                    enemy.TakeDamage(finalDamage);
                    
                    // Apply burning effect
                    if (BurningEffect.prefab != null)
                    {
                        float finalBurnDPS = burnDamagePerSecond * (doubleDamage ? 2f : 1f);
                        BurningEffect.ApplyEffect(hitCollider.gameObject, burnDuration, finalBurnDPS);
                    }
                }
            }
        }
        
        // If we should leave burning ground, create it
        if (leaveBurningGround)
        {
            GameObject burningGround = new GameObject("BurningGround");
            burningGround.transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);
            
            // Add burn zone behavior that damages enemies in the area
            BurningGroundBehavior burningGroundBehavior = burningGround.AddComponent<BurningGroundBehavior>();
            burningGroundBehavior.Initialize(burnDuration, burnDamagePerSecond * (doubleDamage ? 2f : 1f), explosionRadius * 1.5f);
        }
        
        // Spawn explosion effect
        if (explosionEffectPrefab != null)
        {
            // Check if there's a pool for explosion effects
            GameObject explosionEffect;
            
            if (PoolManager.Instance.Pools.ContainsKey(explosionEffectPrefab.name))
            {
                explosionEffect = PoolManager.Instance.Pools[explosionEffectPrefab.name].Get();
                explosionEffect.transform.position = transform.position;
                explosionEffect.transform.rotation = Quaternion.identity;
                
                // Return to pool after effect finishes
                StartCoroutine(ReturnToPoolAfterDelay(explosionEffect, 2f));
            }
            else
            {
                // Fallback to instantiation
                explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                Destroy(explosionEffect, 2f); // Destroy after animation
            }
        }
        
        // Clean up the trail
        if (activeTrail != null)
        {
            Destroy(activeTrail);
            activeTrail = null;
        }
        
        // Return to pool
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }
    
    // Utility method to return an object to the pool after a delay
    private System.Collections.IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
    
    // Reset when disabled (returned to pool)
    private void OnDisable()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}

// Component to handle burning ground behavior
public class BurningGroundBehavior : MonoBehaviour
{
    private float damage;
    private float remainingDuration;
    private float radius;
    private float tickRate = 0.5f; // Apply damage every 0.5 seconds
    private float nextTick = 0f;
    
    public void Initialize(float duration, float damagePerSecond, float radius)
    {
        this.remainingDuration = duration;
        this.damage = damagePerSecond;
        this.radius = radius;
        
        // Add a sphere collider
        SphereCollider col = gameObject.AddComponent<SphereCollider>();
        col.radius = radius;
        col.isTrigger = true;
    }
    
    private void Update()
    {
        // Count down duration
        remainingDuration -= Time.deltaTime;
        if (remainingDuration <= 0)
        {
            Destroy(gameObject);
            return;
        }
        
        // Apply damage at regular intervals
        nextTick -= Time.deltaTime;
        if (nextTick <= 0)
        {
            ApplyDamageToEnemiesInZone();
            nextTick = tickRate;
        }
    }
    
    private void ApplyDamageToEnemiesInZone()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Apply burning effect
                if (BurningEffect.prefab != null)
                {
                    BurningEffect.ApplyEffect(hitCollider.gameObject, tickRate * 2f, damage);
                }
            }
        }
    }
} 