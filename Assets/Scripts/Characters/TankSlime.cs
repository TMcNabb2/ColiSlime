using UnityEngine;

public class TankSlime : Character
{
    [SerializeField] private float healthBonus = 0.30f; // 30% increase
    [SerializeField] private float movementSpeedPenalty = 0.15f; // 15% decrease
    [SerializeField] private float damageReductionAmount = 0.50f; // 50% damage reduction
    [SerializeField] private float damageReductionDuration = 5f; // 5 seconds
    
    private bool damageReductionActive = false;
    private float damageReductionTimer = 0f;
    
    protected override void Awake()
    {
        base.Awake();
        
        // Apply health bonus and movement speed penalty
        AddStatMultiplier("Health", 1 + healthBonus);
        AddStatMultiplier("MovementSpeed", 1 - movementSpeedPenalty);
    }
    
    protected override void Update()
    {
        base.Update();
        
        // Handle damage reduction timer
        if (damageReductionActive)
        {
            damageReductionTimer -= Time.deltaTime;
            if (damageReductionTimer <= 0)
            {
                damageReductionActive = false;
            }
        }
    }
    
    public override void TakeDamage(float amount)
    {
        // Apply damage reduction if active
        if (damageReductionActive)
        {
            amount *= (1 - damageReductionAmount);
        }
        
        base.TakeDamage(amount);
        
        // Activate damage reduction after being hit if not already active
        if (!damageReductionActive)
        {
            ActivateUniqueAbility();
        }
    }
    
    public override void ActivateUniqueAbility()
    {
        if (!CanUseUniqueAbility()) return;
        
        // Activate damage reduction
        damageReductionActive = true;
        damageReductionTimer = damageReductionDuration;
        
        StartUniqueCooldown();
    }
} 