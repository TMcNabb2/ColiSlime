using UnityEngine;
using UnityEngine.Events;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private CharacterStats[] availableCharacters;
    [SerializeField] private Character selectedCharacter;
    
    public static CharacterSelector Instance { get; private set; }
    
    public UnityEvent<CharacterStats> OnCharacterSelected = new UnityEvent<CharacterStats>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SelectCharacter(int index)
    {
        if (index >= 0 && index < availableCharacters.Length)
        {
            CharacterStats selectedStats = availableCharacters[index];
            OnCharacterSelected.Invoke(selectedStats);
        }
    }
    
    public CharacterStats GetSelectedCharacterStats()
    {
        if (selectedCharacter != null)
        {
            return selectedCharacter.BaseStats;
        }
        
        // Default to first character if none selected
        return availableCharacters.Length > 0 ? availableCharacters[0] : null;
    }
} 