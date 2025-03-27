using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager Instance { get; private set; }
    
    // References to other managers
    [SerializeField] private CharacterSelection characterSelection;
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private PoolManager objectPooler;
    
    // Reference to player spawn position
    [SerializeField] private Transform playerSpawnPoint;
    
    // Game state
    public enum GameState
    {
        MainMenu,
        CharacterSelection,
        Playing,
        Paused,
        GameOver
    }
    
    private GameState currentState = GameState.MainMenu;
    
    // Player reference
    [HideInInspector] public GameObject player;
    
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
        
        // Get references if not set in inspector
        if (characterSelection == null)
            characterSelection = FindFirstObjectByType<CharacterSelection>();
        
        if (weaponManager == null)
            weaponManager = FindFirstObjectByType<WeaponManager>();
        
        if (objectPooler == null)
            objectPooler = FindFirstObjectByType<PoolManager>();
    }
    
    private void Start()
    {
        // For testing: Start the game directly
        StartGame("BalancedSlime", "FireWeapon");
    }
    
    // Method to start the game with selected character and weapon
    public void StartGame(string characterType, string weaponType)
    {
        // Change game state
        currentState = GameState.Playing;
        
        // Spawn the player character
        Vector3 spawnPosition = playerSpawnPoint != null ? playerSpawnPoint.position : Vector3.zero;
        Character playerCharacter = characterSelection.SpawnSelectedCharacter(spawnPosition);
        
        if (playerCharacter != null)
        {
            player = playerCharacter.gameObject;
            
            // Equip the weapon
            Weapon weapon = weaponManager.EquipWeapon(weaponType, player.transform);
            
            // Subscribe to player death event
            playerCharacter.OnDeath += HandlePlayerDeath;
        }
        else
        {
            Debug.LogError("Failed to spawn player character!");
        }
    }
    
    // Method to handle player death
    private void HandlePlayerDeath()
    {
        // Game over state
        currentState = GameState.GameOver;
        
        // Show game over UI
        Debug.Log("Game Over!");
        
        // Restart after delay
        StartCoroutine(RestartAfterDelay(3f));
    }
    
    // Coroutine to restart the game after a delay
    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // For now, just restart the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    
    // Method to trigger all weapon ultimates
    public void TriggerUltimates()
    {
        if (currentState == GameState.Playing && weaponManager != null)
        {
            weaponManager.TriggerAllUltimates();
        }
    }
    
    // Method to pause the game
    public void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0f;
        }
    }
    
    // Method to resume the game
    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            Time.timeScale = 1f;
        }
    }
} 