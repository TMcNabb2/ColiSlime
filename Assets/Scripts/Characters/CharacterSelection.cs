using UnityEngine;
using System.Collections.Generic;

public class CharacterSelection : MonoBehaviour
{
    // Singleton instance
    public static CharacterSelection Instance { get; private set; }
    
    // References to character prefabs
    [SerializeField] private GameObject speedSlimePrefab;
    [SerializeField] private GameObject tankSlimePrefab;
    [SerializeField] private GameObject elementalSlimePrefab;
    [SerializeField] private GameObject shadowSlimePrefab;
    [SerializeField] private GameObject balancedSlimePrefab;
    [SerializeField] private GameObject poisonSlimePrefab;
    
    // Currently selected character type
    private GameObject selectedCharacterPrefab;
    
    // Track all available character types
    private Dictionary<string, GameObject> characterPrefabs = new Dictionary<string, GameObject>();
    
    // Currently active character
    [HideInInspector]
    public Character activeCharacter;
    
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
        
        // Initialize character prefabs dictionary
        if (speedSlimePrefab != null) characterPrefabs.Add("SpeedSlime", speedSlimePrefab);
        if (tankSlimePrefab != null) characterPrefabs.Add("TankSlime", tankSlimePrefab);
        if (elementalSlimePrefab != null) characterPrefabs.Add("ElementalSlime", elementalSlimePrefab);
        if (shadowSlimePrefab != null) characterPrefabs.Add("ShadowSlime", shadowSlimePrefab);
        if (balancedSlimePrefab != null) characterPrefabs.Add("BalancedSlime", balancedSlimePrefab);
        if (poisonSlimePrefab != null) characterPrefabs.Add("PoisonSlime", poisonSlimePrefab);
        
        // Default to balanced slime if nothing is selected
        if (selectedCharacterPrefab == null && balancedSlimePrefab != null)
        {
            selectedCharacterPrefab = balancedSlimePrefab;
        }
    }
    
    // Method to select a character type
    public void SelectCharacter(string characterType)
    {
        if (characterPrefabs.ContainsKey(characterType))
        {
            selectedCharacterPrefab = characterPrefabs[characterType];
            Debug.Log($"Selected character: {characterType}");
        }
        else
        {
            Debug.LogError($"Character type not found: {characterType}");
        }
    }
    
    // Method to spawn the selected character at a position
    public Character SpawnSelectedCharacter(Vector3 position)
    {
        if (selectedCharacterPrefab == null)
        {
            Debug.LogError("No character selected!");
            return null;
        }
        
        GameObject characterInstance = Instantiate(selectedCharacterPrefab, position, Quaternion.identity);
        activeCharacter = characterInstance.GetComponent<Character>();
        
        return activeCharacter;
    }
    
    // Method to get all available character types
    public List<string> GetAvailableCharacterTypes()
    {
        return new List<string>(characterPrefabs.Keys);
    }
} 