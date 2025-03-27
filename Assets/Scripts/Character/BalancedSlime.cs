using UnityEngine;

[CreateAssetMenu(fileName = "BalancedSlime", menuName = "Scriptable Objects/Character/BalancedSlime")]
public class BalancedSlime : CharacterStats
{
    private void OnEnable()
    {
        // Set character info
        characterName = "Balanced Slime";
        description = "A jack-of-all-trades option with 15% increased XP gain and a 10% chance to choose another ability on level up.";
        
        // Set base stats (can be adjusted for balance)
        maxHealth = 100f; 
        movementSpeed = 5f;
        xpMultiplier = 1.15f; // 15% XP bonus
        
        // Set special abilities
        hasAdaptation = true;
        adaptationChance = 0.1f; // 10% chance
    }
} 