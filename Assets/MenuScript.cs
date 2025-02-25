using UnityEngine;
using UnityEngine.UIElements;

public class MenuScript : MonoBehaviour
{
    private UIDocument _uiDocument;
    private bool _isPaused = false;

    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;
        var pauseScreen = root.Q<VisualElement>("PauseScreen").visible = false;
        var startButton = root.Q<Button>("StartButton");
        var continueButton = root.Q<Button>("ContinueButton");
        var optionsButton = root.Q<Button>("SettingsButton");
        var pOptionsButton = root.Q<Button>("PauseSettingsButton");

        var quitToMenuButton = root.Q<Button>("QuitMenuButton");
        var quitButton = root.Q<Button>("QuitButton");

        startButton.clicked += StartGame;
        continueButton.clicked += ContinueGame;
        optionsButton.clicked += OpenOptions;
        pOptionsButton.clicked += OpenOptions;
        quitToMenuButton.clicked += QuitToMenu;
        quitButton.clicked += QuitGame;
    }

    void StartGame()
    {
        Debug.Log("Game started");
    }
    void ContinueGame()
    {
        Debug.Log("Game resumed");
        _uiDocument.rootVisualElement.Q<VisualElement>("PauseScreen").visible = false;
        _isPaused = false;
        Time.timeScale = 1;

    }

    private void OpenOptions()
    {
        Debug.Log("Options opened");
    }

    private void QuitToMenu()
    {
        Debug.Log("Quit to menu");
    }
    private void QuitGame()
    {
        Debug.Log("Quit the game");
    }
    public bool IsPaused()
    {
        return _isPaused;
    }
}
