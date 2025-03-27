using UnityEngine;
using JDoddsNAIT.Stats;

public class SpeedSlime : Character
{
    [SerializeField] private float movementSpeedBonus = 0.15f; // 15% increase
    [SerializeField] private float healthPenalty = 0.15f; // 15% decrease
    [SerializeField] private float cooldownReductionPerLevel = 0.01f; // 1% per level
    
    private int level = 1;
    
    protected override void Awake()
    {
        base.Awake();
        
        // Apply speed bonus and health penalty using stat modifiers
        AddStatMultiplier("MovementSpeed", 1 + movementSpeedBonus);
        AddStatMultiplier("Health", 1 - healthPenalty);
        
        // Apply cooldown reduction based on level
        ApplyCooldownReduction();
    }
    
    public void LevelUp()
    {
        level++;
        ApplyCooldownReduction();
    }
    
    private void ApplyCooldownReduction()
    {
        // Apply level-based cooldown reduction
        float reduction = level * cooldownReductionPerLevel;
        
        // Remove any existing cooldown reduction modifiers with this tag
        // Note: In a more robust implementation, you would track and remove specific modifiers
        
        // Apply the new level-based cooldown reduction 
        AddStatMultiplier("CooldownReduction", reduction);
    }
    
    public override void ActivateUniqueAbility()
    {
        if (!CanUseUniqueAbility()) return;
        
        // Speed burst ability - temporarily increase speed even more
        // Add temporary multiplier that will expire after 3 seconds
        AddStatMultiplier("MovementSpeed", 1.5f, 3f);
        
        StartUniqueCooldown();
    }
} 