using UnityEngine;

public class ShadowSlime : Character
{
    [SerializeField] private float dodgeBonus = 0.10f; // 10% dodge chance
    [SerializeField] private float movementSpeedBonus = 0.10f; // 10% speed increase
    [SerializeField] private float dodgeBoostAmount = 0.50f; // 50% extra dodge chance
    [SerializeField] private float dodgeBoostDuration = 5f; // 5 seconds
    
    private bool dodgeBoostActive = false;
    private float dodgeBoostTimer = 0f;
    private float totalDodgeChance = 0f;
    
    protected override void Awake()
    {
        base.Awake();
        // Put your initialization code here
        // Use AddStatModifier() or AddStatMultiplier() instead of direct assignment
        AddStatMultiplier("Health", 1.1f, 5f); // Apply 10% boost for 5 seconds
    
    
        // Apply dodge and movement speed bonuses
        totalDodgeChance = dodgeBonus;
        AddStatMultiplier("MovementSpeed", 1 + movementSpeedBonus);
    }
    
    protected override void Update()
    {
        base.Update();
        
        // Handle dodge boost timer
        if (dodgeBoostActive)
        {
            dodgeBoostTimer -= Time.deltaTime;
            if (dodgeBoostTimer <= 0)
            {
                DeactivateDodgeBoost();
            }
        }
    }
    
    public override void TakeDamage(float amount)
    {
        // Check for dodge
        if (Random.value < totalDodgeChance)
        {
            // Dodge successful - no damage taken
            Debug.Log("Attack dodged!");
            
            // Activate dodge boost after being hit
            ActivateUniqueAbility();
            return;
        }
        
        base.TakeDamage(amount);
    }
    
    private void ActivateDodgeBoost()
    {
        totalDodgeChance = dodgeBonus + dodgeBoostAmount;
        dodgeBoostActive = true;
        dodgeBoostTimer = dodgeBoostDuration;
    }
    
    private void DeactivateDodgeBoost()
    {
        totalDodgeChance = dodgeBonus;
        dodgeBoostActive = false;
    }
    
    public override void ActivateUniqueAbility()
    {
        if (!CanUseUniqueAbility()) return;
        
        // Shadow step - boost dodge chance
        ActivateDodgeBoost();
        
        StartUniqueCooldown();
    }
} 