using UnityEngine;
using System.Collections.Generic;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float cooldown = 1f;
    [SerializeField] protected float range = 10f;
    [SerializeField] protected LayerMask targetLayers;
    
    // For upgrades/abilities
    [SerializeField] protected List<WeaponAbility> abilities = new List<WeaponAbility>();
    
    // Ultimate ability
    [SerializeField] protected float ultimateCooldown = 20f;
    [SerializeField] protected bool ultimateUnlocked = false;
    protected float currentUltimateCooldown = 0f;
    
    // Tracking cooldown
    protected float currentCooldown = 0f;
    
    // Add virtual Awake method
    protected virtual void Awake()
    {
        // Base initialization
    }
    
    protected virtual void Update()
    {
        // Update cooldowns
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
        
        if (currentUltimateCooldown > 0)
        {
            currentUltimateCooldown -= Time.deltaTime;
        }
        
        // Auto attack if possible
        if (currentCooldown <= 0)
        {
            TryAttack();
        }
    }
    
    // Method to execute an attack
    protected abstract void TryAttack();
    
    // Method to trigger ultimate ability
    public virtual void TriggerUltimate()
    {
        if (!ultimateUnlocked || currentUltimateCooldown > 0) return;
        
        UseUltimateAbility();
        currentUltimateCooldown = ultimateCooldown;
    }
    
    // Ultimate ability implementation
    protected abstract void UseUltimateAbility();
    
    // Apply ability effects
    protected virtual void ApplyAbilityEffects(WeaponAbility ability) {}
    
    // Check if the weapon has a specific ability
    public bool HasAbility(string abilityName)
    {
        return abilities.Exists(a => a.abilityName == abilityName);
    }
    
    // Method to unlock the ultimate ability
    public void UnlockUltimate()
    {
        ultimateUnlocked = true;
    }
}

// Base class for weapon abilities
[System.Serializable]
public class WeaponAbility
{
    public string abilityName;
    public string description;
    
    // Different ability types can provide different bonuses
    public float damageBonusMultiplier = 1f;
    public float cooldownReductionMultiplier = 1f;
    public float rangeBonusMultiplier = 1f;
    
    // Constructor
    public WeaponAbility(string name, string desc)
    {
        abilityName = name;
        description = desc;
    }
}
