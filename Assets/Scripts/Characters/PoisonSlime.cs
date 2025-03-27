using UnityEngine;

public class PoisonSlime : Character
{
    [SerializeField] private float poisonDamagePerStack = 2f; // Damage per poison stack
    [SerializeField] private float poisonDuration = 5f; // Duration of poison effect
    [SerializeField] private float poisonCloudRadius = 3f; // Radius of poison cloud
    [SerializeField] private float poisonCloudDuration = 5f; // Duration of poison cloud
    
    protected override void Awake()
    {
        base.Awake();
        // Put your initialization code here
        // Use AddStatModifier() or AddStatMultiplier() instead of direct assignment
        AddStatMultiplier("Health", 1.1f, 5f); // Apply 10% boost for 5 seconds
    }
    
    // Method to apply poison to an enemy
    public void ApplyPoisonStack(GameObject target, int stacks = 1)
    {
        if (PoisonEffect.prefab != null)
        {
            for (int i = 0; i < stacks; i++)
            {
                // Apply poison effect with stacking damage
                PoisonEffect.ApplyEffect(target, poisonDuration, poisonDamagePerStack);
            }
        }
    }
    
    // Method to create a poison cloud at a position
    public void CreatePoisonCloud(Vector3 position)
    {
        GameObject poisonCloud = new GameObject("PoisonCloud");
        poisonCloud.transform.position = position;
        
        // Add a sphere collider to detect enemies
        SphereCollider cloudCollider = poisonCloud.AddComponent<SphereCollider>();
        cloudCollider.radius = poisonCloudRadius;
        cloudCollider.isTrigger = true;
        
        // Add the cloud behavior component
        PoisonCloudBehavior cloudBehavior = poisonCloud.AddComponent<PoisonCloudBehavior>();
        cloudBehavior.Initialize(this, poisonDamagePerStack, poisonCloudDuration);
        
        // Destroy the cloud after duration
        Destroy(poisonCloud, poisonCloudDuration);
    }
    
    public override void ActivateUniqueAbility()
    {
        if (!CanUseUniqueAbility()) return;
        
        // Toxicity Burst - apply poison to all nearby enemies
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Apply multiple poison stacks
                ApplyPoisonStack(hitCollider.gameObject, 3);
            }
        }
        
        StartUniqueCooldown();
    }
}

// Component to handle poison cloud behavior
public class PoisonCloudBehavior : MonoBehaviour
{
    private PoisonSlime owner;
    private float damage;
    private float tickRate = 0.5f; // Apply damage every 0.5 seconds
    private float nextTick = 0f;
    
    public void Initialize(PoisonSlime owner, float damage, float duration)
    {
        this.owner = owner;
        this.damage = damage;
    }
    
    private void Update()
    {
        nextTick -= Time.deltaTime;
        if (nextTick <= 0)
        {
            ApplyDamageToEnemiesInCloud();
            nextTick = tickRate;
        }
    }
    
    private void ApplyDamageToEnemiesInCloud()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Apply poison effect
                owner.ApplyPoisonStack(hitCollider.gameObject);
            }
        }
    }
} 