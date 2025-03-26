using UnityEditor.Media;
using UnityEngine;
// <summary>
/// Manages the settings data and controls the flow of this data 
/// between the SaveManager and the UI.
/// </summary>
public class SettingsScreenController : MonoBehaviour
{
    GameData m_SettingsData;

    [SerializeField] bool m_debug;

    void OnEnable()
    {
        SettingsEvents.UIGameDataUpdated += OnUISettingsUpdated;
        SaveManager.GameDataLoaded += OnGameDataLoaded;
    }
    private void OnDisable()
    {
        SettingsEvents.UIGameDataUpdated -= OnUISettingsUpdated;
        SaveManager.GameDataLoaded -= OnGameDataLoaded;
    }
    /// <summary>
    /// Handle updated Settings Data
    /// </summary>
    /// <param name="newSettingsData"></param>

    void OnUISettingsUpdated(GameData newSettingsData)
    {
        if (newSettingsData == null)
            return;

        m_SettingsData = newSettingsData;

        // Toggle the Fps Counter based on slide toggle position

        // Notify the GameDataManager and other listeners
        SettingsEvents.SettingsUpdated?.Invoke(m_SettingsData);
    }
    /// <summary>
    /// Sync loaded data from SaveManager to UI
    /// </summary>
    /// <param name="gameData"></param>
    void OnGameDataLoaded(GameData gameData)
    {
        if (gameData == null)
            return;

        m_SettingsData = gameData;
        SettingsEvents.GameDataLoaded?.Invoke(m_SettingsData);
    }
}