using UnityEngine;

public class BalancedSlime : Character
{
    [SerializeField] private float xpBonus = 0.15f; // 15% XP bonus
    [SerializeField] private float extraAbilityChance = 0.10f; // 10% chance for extra ability
    
    protected override void Awake()
    {
        base.Awake();
        // Put your initialization code here
        // Use AddStatModifier() or AddStatMultiplier() instead of direct assignment
        AddStatMultiplier("Health", 1.1f, 5f); // Apply 10% boost for 5 seconds
    }
        
    
    
    // Method to be called when gaining XP
    public float CalculateXpGain(float baseXpAmount)
    {
        return baseXpAmount * (1 + xpBonus);
    }
    
    // Method to check if player should get an extra ability option on level up
    public bool ShouldGetExtraAbilityChoice()
    {
        return Random.value < extraAbilityChance;
    }
    
    public override void ActivateUniqueAbility()
    {
        if (!CanUseUniqueAbility()) return;
        
        // Balanced Nature - temporarily boost all stats mildly
        AddStatMultiplier("Health", 1.1f);
        AddStatMultiplier("MovementSpeed", 1.1f);
        AddStatMultiplier("Damage", 1.1f);
        
        // Reset after 5 seconds
        Invoke(nameof(ResetStats), 5f);
        
        StartUniqueCooldown();
    }
    
    private void ResetStats()
    {
        // Reset stats to base values
        AddStatMultiplier("Health", 1.0f);
        AddStatMultiplier("MovementSpeed", 1.0f);
        AddStatMultiplier("Damage", 1.0f);
    }
} 