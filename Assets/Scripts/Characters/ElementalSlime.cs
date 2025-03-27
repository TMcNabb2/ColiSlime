using UnityEngine;
using System.Collections.Generic;

public class ElementalSlime : Character
{
    public enum ElementType
    {
        Fire,   // Burn effect
        Ice,    // Slow effect
        Lightning // Paralyze effect
    }
    
    [SerializeField] private float abilityRecastChance = 0.05f; // 5% chance to recast
    private string lastUsedAbilityName = "";
    
    protected override void Awake()
    {
        base.Awake();
        
        // Put your initialization code here
        // Use AddStatModifier() or AddStatMultiplier() instead of direct assignment
        AddStatMultiplier("Health", 1.1f, 5f); // Apply 10% boost for 5 seconds
    }
    
    // Apply random elemental effect to attacks
    public ElementType GetRandomElementType()
    {
        return (ElementType)Random.Range(0, System.Enum.GetValues(typeof(ElementType)).Length);
    }
    
    // Method to apply elemental effects to enemies
    public void ApplyElementalEffect(GameObject target, ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                // Apply burn effect (implemented in BurningEffect.cs)
                if (BurningEffect.prefab != null)
                {
                    BurningEffect.ApplyEffect(target, 5f, 2f); // Duration 5 seconds, 2 damage per tick
                }
                break;
                
            case ElementType.Ice:
                // Apply slow effect
                // This would be implemented similar to burning effect
                Debug.Log("Applied Ice slow effect to " + target.name);
                break;
                
            case ElementType.Lightning:
                // Apply paralyze effect
                // This would be implemented similar to burning effect
                Debug.Log("Applied Lightning paralyze effect to " + target.name);
                break;
        }
    }
    
    // Check if ability should be recast
    public bool ShouldRecastAbility()
    {
        return Random.value < abilityRecastChance;
    }
    
    // Record the last used ability
    public void RecordAbilityUse(string abilityName)
    {
        lastUsedAbilityName = abilityName;
    }
    
    public override void ActivateUniqueAbility()
    {
        if (!CanUseUniqueAbility()) return;
        
        // Elemental surge - apply all elemental effects at once to nearby enemies
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Apply all elemental effects
                ApplyElementalEffect(hitCollider.gameObject, ElementType.Fire);
                ApplyElementalEffect(hitCollider.gameObject, ElementType.Ice);
                ApplyElementalEffect(hitCollider.gameObject, ElementType.Lightning);
            }
        }
        
        StartUniqueCooldown();
    }
} 