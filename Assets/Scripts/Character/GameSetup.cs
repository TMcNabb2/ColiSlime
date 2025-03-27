using UnityEngine;

public static class GameSetup
{
    // Set up a new game with the selected character and initial weapon
    public static void SetupNewGame(CharacterStats characterStats, GameObject playerPrefab, Transform spawnPoint)
    {
        // Spawn the player at the spawn point
        GameObject playerObj = Object.Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        
        // Add and set up the Character component
        Character character = playerObj.GetComponent<Character>();
        if (character == null)
        {
            character = playerObj.AddComponent<Character>();
        }
        
        // Set the character's stats
        System.Type statsType = characterStats.GetType();
        
        // Get the private _baseStats field using reflection
        System.Reflection.FieldInfo baseStatsField = typeof(JDoddsNAIT.Stats.Stats<CharacterStats>)
            .GetField("_baseStats", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (baseStatsField != null)
        {
            // Create an instance of the specific character stats
            CharacterStats statsInstance = (CharacterStats)ScriptableObject.CreateInstance(statsType);
            
            // Set the field value
            baseStatsField.SetValue(character, statsInstance);
            
            Debug.Log($"Set up character: {statsInstance.characterName}");
        }
        else
        {
            Debug.LogError("Could not find _baseStats field!");
        }
        
        // Add the initial weapon (fire weapon)
        WeaponManager weaponManager = playerObj.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.AddWeapon(0); // Assuming 0 is the index of the fire weapon
        }
        else
        {
            Debug.LogWarning("Player doesn't have a WeaponManager component!");
        }
    }
} 