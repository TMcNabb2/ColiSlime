using System;
using UnityEngine;

[RequireComponent(typeof(SaveManager))]
public class GameDataManager : MonoBehaviour
{
    public static Action GameDataRequested;
    public static event Action<GameData> GameDataReceived;

    [SerializeField] GameData m_gameData;
    public GameData GameData { set { m_gameData = value; } get { return m_gameData; } }

    SaveManager m_saveManager;
    //bool m_isDataLoaded;

    void OnEnable()
    {
        //MainMenuUIEvents.HomeScreenShown += OnHomeScreenShown;
        SettingsEvents.SettingsUpdated += OnSettingsUpdated;

        GameDataRequested += OnGameDataRequested;
    }
    private void OnDisable()
    {
        //MainMenuUIEvents.HomeScreenShown -= OnHomeScreenShown;
        SettingsEvents.SettingsUpdated -= OnSettingsUpdated;
        GameDataRequested -= OnGameDataRequested;
    }
    private void Awake()
    {
        m_saveManager = GetComponent<SaveManager>();
    }

    private void Start()
    {
        m_saveManager.LoadGame();
        //m_isDataLoaded = true;
    }
    void OnGameDataRequested()
    {
        GameDataReceived?.Invoke(m_gameData);
    }
    void OnSettingsUpdated(GameData gameData)
    {
        if (gameData == null)
            return;
        m_gameData.SfxVolume = gameData.SfxVolume;
        m_gameData.MusicVolume = gameData.MusicVolume;
        m_gameData.IsToggled = gameData.IsToggled;

    }

}