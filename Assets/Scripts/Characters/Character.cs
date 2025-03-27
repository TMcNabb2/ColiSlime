using UnityEngine;
using JDoddsNAIT.Stats;
using System;

// Create a ScriptableObject for character stats
[CreateAssetMenu(fileName = "CharacterStats", menuName = "Character/CharacterStats")]
public class CharacterBaseStats : ScriptableObject, IStatContainer
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _cooldownReduction = 0f; // Value between 0-1 (percentage)
    [SerializeField] private float _dodgeChance = 0f; // Value between 0-1 (percentage)
    
    // Query system for Stats
    public Query GetBaseValue(string statName)
    {
        switch (statName)
        {
            case "Health":
                return new Query(statName, _health);
            case "MovementSpeed":
                return new Query(statName, _movementSpeed);
            case "Damage":
                return new Query(statName, _damage);
            case "CooldownReduction":
                return new Query(statName, _cooldownReduction);
            case "DodgeChance":
                return new Query(statName, _dodgeChance);
            default:
                Debug.LogWarning($"Stat '{statName}' not found in CharacterBaseStats.");
                return new Query(statName, 0f);
        }
    }
}

public abstract class Character : Stats<CharacterBaseStats>
{
    // Character-specific unique ability cooldown
    [SerializeField] protected float uniqueAbilityCooldown = 5f;
    protected float currentUniqueCooldown = 0f;
    
    // Events
    public event Action<float> OnHealthChanged;
    public event Action OnDeath;
    
    // Easy access to stats
    public float Health => GetStat<float>("Health");
    public float MovementSpeed => GetStat<float>("MovementSpeed");
    public float Damage => GetStat<float>("Damage");
    public float CooldownReduction => GetStat<float>("CooldownReduction");
    public float DodgeChance => GetStat<float>("DodgeChance");
    
    // Current health tracking
    private float _currentHealth;
    
    protected override void Awake()
    {
        base.Awake();
        _currentHealth = Health;
    }
    
    protected virtual void Update()
    {
        if (currentUniqueCooldown > 0)
        {
            currentUniqueCooldown -= Time.deltaTime;
        }
    }
    
    public virtual void TakeDamage(float amount)
    {
        // Check for dodge chance
        if (UnityEngine.Random.value <= DodgeChance)
        {
            Debug.Log("Attack dodged!");
            return;
        }
        
        _currentHealth -= amount;
        OnHealthChanged?.Invoke(_currentHealth);
        
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    
    protected virtual void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
    
    // Unique ability that each character type overrides
    public abstract void ActivateUniqueAbility();
    
    public bool CanUseUniqueAbility()
    {
        return currentUniqueCooldown <= 0;
    }
    
    protected void StartUniqueCooldown()
    {
        // Apply cooldown reduction to the ability cooldown
        currentUniqueCooldown = uniqueAbilityCooldown * (1f - CooldownReduction);
    }
    
    // Helper method to add a new stat modifier
    public void AddStatModifier(string statName, float value, float duration = 0)
    {
        if (duration <= 0)
        {
            // Permanent modifier
            AddModifier(new FloatAddModifier(statName, value));
        }
        else
        {
            // Temporary modifier with duration
            AddModifier(new FloatAddModifier(statName, value, duration));
        }
    }
    
    // Helper method to add a percentage modifier
    public void AddStatMultiplier(string statName, float multiplier, float duration = 0)
    {
        if (duration <= 0)
        {
            // Permanent modifier
            AddModifier(new FloatMultiplyModifier(statName, multiplier));
        }
        else
        {
            // Temporary modifier with duration
            AddModifier(new FloatMultiplyModifier(statName, multiplier, duration));
        }
    }
}

// Example stat modifiers
public class FloatAddModifier : StatModifier<float>
{
    private float _addAmount;
    
    public FloatAddModifier(string statName, float amount, float duration = 0) : base(duration, statName)
    {
        _addAmount = amount;
    }
    
    protected override void OnHandle(object sender, Query<float> query)
    {
        query.Value += _addAmount;
    }
}

public class FloatMultiplyModifier : StatModifier<float>
{
    private float _multiplier;
    
    public FloatMultiplyModifier(string statName, float multiplier, float duration = 0) : base(duration, statName)
    {
        _multiplier = multiplier;
    }
    
    protected override void OnHandle(object sender, Query<float> query)
    {
        query.Value *= _multiplier;
    }
} 