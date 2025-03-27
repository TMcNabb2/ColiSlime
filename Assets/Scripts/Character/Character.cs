using UnityEngine;
using JDoddsNAIT.Stats;

public class Character : Stats<CharacterStats>
{
    private Health health;
    public bool HasAdaptation => BaseStats.hasAdaptation;

    protected override void Awake()
    {
        base.Awake();
        health = GetComponent<Health>();
        
        if (health != null)
        {
            // Set the health component values based on character stats
            health.SetMaxHealth(GetStat<float>("MaxHealth"));
        }
    }

    // Apply XP with character's XP multiplier
    public float ApplyXPMultiplier(float baseXP)
    {
        return baseXP * GetStat<float>("XPMultiplier");
    }

    // Check if adaptation triggers
    public bool CheckAdaptation()
    {
        if (!HasAdaptation) return false;
        
        float chance = GetStat<float>("AdaptationChance");
        return Random.value <= chance;
    }
} 