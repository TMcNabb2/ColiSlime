using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Singleton instance
    public static WeaponManager Instance { get; private set; }
    
    // References to weapon prefabs
    [SerializeField] private GameObject fireWeaponPrefab;
    // Add more weapon types here as needed
    
    // Currently active weapons
    [HideInInspector] public List<Weapon> activeWeapons = new List<Weapon>();
    
    // Available abilities for each weapon type
    private Dictionary<string, List<WeaponAbility>> weaponAbilities = new Dictionary<string, List<WeaponAbility>>();
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Initialize weapon abilities
        InitializeWeaponAbilities();
    }
    
    private void InitializeWeaponAbilities()
    {
        // Fire weapon abilities
        List<WeaponAbility> fireAbilities = new List<WeaponAbility>
        {
            new WeaponAbility("Flame Shot", "Shoots a fireball that explodes on impact, burning enemies"),
            new WeaponAbility("Flame Burst", "Increases the impact area of the fireball and leaves a patch of burning ground behind"),
            new WeaponAbility("Molten Core", "Doubles the effects of fire abilities (2 Fireballs, double DOT dmg)"),
            new WeaponAbility("Meteor Drop", "Fireballs transform into meteors that drop onto enemies and roll away from you, dealing damage in a line and leaving fire behind"),
            new WeaponAbility("Hellscape", "Ultimate: Unleashes a rain of meteors, setting the screen ablaze for 5s")
        };
        
        weaponAbilities.Add("FireWeapon", fireAbilities);
        
        // Add more weapon types here as needed
    }
    
    // Method to equip a weapon to the player
    public Weapon EquipWeapon(string weaponType, Transform weaponParent)
    {
        GameObject weaponPrefab = null;
        
        // Select the appropriate weapon prefab
        switch (weaponType)
        {
            case "FireWeapon":
                weaponPrefab = fireWeaponPrefab;
                break;
            // Add cases for other weapon types
        }
        
        if (weaponPrefab == null)
        {
            Debug.LogError($"Weapon type not found: {weaponType}");
            return null;
        }
        
        // Instantiate the weapon as a child of the weapon parent
        GameObject weaponInstance = Instantiate(weaponPrefab, weaponParent);
        Weapon weapon = weaponInstance.GetComponent<Weapon>();
        
        if (weapon != null)
        {
            activeWeapons.Add(weapon);
        }
        else
        {
            Debug.LogError($"Weapon component not found on prefab: {weaponType}");
        }
        
        return weapon;
    }
    
    // Method to get available abilities for a weapon type
    public List<WeaponAbility> GetAbilitiesForWeapon(string weaponType)
    {
        if (weaponAbilities.ContainsKey(weaponType))
        {
            return weaponAbilities[weaponType];
        }
        
        return new List<WeaponAbility>();
    }
    
    // Method to get a random ability for a weapon type that the weapon doesn't already have
    public WeaponAbility GetRandomAbility(Weapon weapon, string weaponType)
    {
        if (!weaponAbilities.ContainsKey(weaponType))
        {
            return null;
        }
        
        List<WeaponAbility> availableAbilities = new List<WeaponAbility>();
        
        // Find abilities that the weapon doesn't already have
        foreach (WeaponAbility ability in weaponAbilities[weaponType])
        {
            if (!weapon.HasAbility(ability.abilityName))
            {
                availableAbilities.Add(ability);
            }
        }
        
        // Return a random ability from the available ones
        if (availableAbilities.Count > 0)
        {
            return availableAbilities[Random.Range(0, availableAbilities.Count)];
        }
        
        return null;
    }
    
    // Method to trigger ultimates for all active weapons
    public void TriggerAllUltimates()
    {
        foreach (Weapon weapon in activeWeapons)
        {
            weapon.TriggerUltimate();
        }
    }
} 