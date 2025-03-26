using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(GameDataManager))]
public class SaveManager : MonoBehaviour
{
    public static event Action<GameData> GameDataLoaded;

    [Tooltip("Filename to save game and settings data")]
    [SerializeField] private string m_SaveFilename = "savegame.dat";
    [Tooltip("Show Debug Messages.")]
    [SerializeField] bool m_Debug;

    GameDataManager m_GameDataManager;

    private void Awake()
    {
        m_GameDataManager = GetComponent<GameDataManager>();
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }
    void OnEnable()
    {
        SettingsEvents.SettingsShown += OnSettingsShown;
        SettingsEvents.SettingsUpdated += OnSettingsUpdated;

        GameplayEvents.SettingsUpdated += OnSettingsUpdated;
    }
    void OnDisable()
    {
        SettingsEvents.SettingsShown -= OnSettingsShown;
        SettingsEvents.SettingsUpdated -= OnSettingsUpdated;
        GameplayEvents.SettingsUpdated -= OnSettingsUpdated;
    }
    public GameData NewGame()
    {
        return new GameData();
    }

    public void LoadGame()
    {
        if (m_GameDataManager.GameData == null)
        {
            if (m_Debug)
            {
                Debug.Log("GAME DATA MANAGER LoadGame: Initializing game data.");
            }
            m_GameDataManager.GameData = NewGame();
        }
        else if (FileManager.LoadFromFile(m_SaveFilename, out var jsonString))
        {
            m_GameDataManager.GameData.LoadJson(jsonString);
            if (m_Debug)
            {
                Debug.Log("GAME DATA MANAGER LoadGame: Game data loaded.");
            }
        }
        if (m_GameDataManager.GameData != null)
        {
            GameDataLoaded?.Invoke(m_GameDataManager.GameData);
        }
    }

    public void SaveGame()
    {
        string jsonFile = m_GameDataManager.GameData.ToJson();

        // save to disk with FileDataHandler
        if (FileManager.WriteToFile(m_SaveFilename, jsonFile) && m_Debug)
        {
            Debug.Log("SaveManager.SaveGame: " + m_SaveFilename + " json string: " + jsonFile);
        }
    }
    void OnSettingsShown()
    {
        if (m_GameDataManager.GameData != null)
        {
            GameDataLoaded?.Invoke(m_GameDataManager.GameData);
        }
    }
    void OnSettingsUpdated(GameData gameData)
    {
        m_GameDataManager.GameData = gameData;
        SaveGame();
    }
}
