using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameScreenController : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string _gameSceneName = "GameScene";
    [SerializeField] private string _mainMenuSceneName = "MainMenuScene";
    private Button abilityOneButton;
    private Button abilityTwoButton;
    [SerializeField] private UIDocument m_mainMenuDocument;
    private VisualElement root;
    private bool isPaused = false;
    private bool isColorChanging = false;

    public static event Action GameWon;
    public static event Action GameLost;
    public static event Action GamePaused;
    public static event Action GameResumed;
    void OnEnable()
    {
        root = m_mainMenuDocument.rootVisualElement;
        abilityOneButton = root.Q<Button>("game-hud-ability__button-1");
        abilityTwoButton = root.Q<Button>("game-hud-ability__button-2");

        abilityOneButton.clicked += OnAbilityOneUsed;
        //abilityTwoButton.clicked += OnAbilityTwoUsed;
        abilityTwoButton.clicked += OnAbilityTwoUsedAction;

        GamePlayEvents.TogglePause += OnTogglePause;

        GamePlayEvents.GameQuit += OnGameQuit;
        GamePlayEvents.GameRestart += OnGameRestarted;

        GamePlayEvents.AbilityOneUsed += OnAbilityOneUsed;
        //GamePlayEvents.AbilityTwoUsed += OnAbilityTwoUsed;
        GamePlayEvents.AbilityTwoUsed += OnAbilityTwoUsedAction;
    }

    void OnDisable()
    {
        GamePlayEvents.TogglePause -= OnTogglePause;


        GamePlayEvents.GameQuit -= OnGameQuit;
        GamePlayEvents.GameRestart -= OnGameRestarted;

        GamePlayEvents.AbilityOneUsed -= OnAbilityOneUsed;
        //GamePlayEvents.AbilityTwoUsed -= OnAbilityTwoUsed;
        GamePlayEvents.AbilityTwoUsed -= OnAbilityTwoUsedAction;

        abilityOneButton.clicked -= OnAbilityOneUsed;
        //abilityTwoButton.clicked -= OnAbilityTwoUsed;
        abilityTwoButton.clicked -= OnAbilityTwoUsedAction;

    }

    void OnDestroy()
    {
        GamePlayEvents.TogglePause -= OnTogglePause;

        GamePlayEvents.GameQuit -= OnGameQuit;
        GamePlayEvents.GameRestart -= OnGameRestarted;

        GamePlayEvents.AbilityOneUsed -= OnAbilityOneUsed;
        //GamePlayEvents.AbilityTwoUsed -= OnAbilityTwoUsed;
        GamePlayEvents.AbilityTwoUsed -= OnAbilityTwoUsedAction;

    }
    public void OnAbilityOneUsed()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Renderer playerRenderer = player.GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                StartCoroutine(ChangeColorTemporarily(playerRenderer));
            }
        }
    }

    private IEnumerator ChangeColorTemporarily(Renderer targetRenderer)
    {
        if (isColorChanging) yield break;
        isColorChanging = true;
        Material originalMaterial = targetRenderer.material;
        Material tempMaterial = new Material(originalMaterial);
        targetRenderer.material = tempMaterial;
        tempMaterial.color = Color.green;
        yield return new WaitForSeconds(1f);
        targetRenderer.material = originalMaterial;
        Destroy(tempMaterial);
        isColorChanging = false;
    }
    private IEnumerator ChangeColorTemporarilyTwo(Renderer targetRenderer)
    {
        if (isColorChanging) yield break;
        isColorChanging = true;
        Material originalMaterial = targetRenderer.material;
        Material tempMaterial = new Material(originalMaterial);
        targetRenderer.material = tempMaterial;
        tempMaterial.color = Color.red;
        yield return new WaitForSeconds(1f);
        targetRenderer.material = originalMaterial;
        Destroy(tempMaterial);
        isColorChanging = false;
    }
    public void OnAbilityTwoUsed()
    {
        // GameObject player = GameObject.FindGameObjectWithTag("Player");
        // if (player != null)
        // {
        //     Renderer playerRenderer = player.GetComponent<Renderer>();
        //     if (playerRenderer != null)
        //     {
        //         StartCoroutine(ChangeColorTemporarilyTwo(playerRenderer));
        //     }
        // }
        
    }
    public void XPUp(float val)
    {
        XPManager.Instance.XP += val;
        Debug.Log($"XP increased by {val}. New XP: {XPManager.Instance.XP}");
    }

    public void OnAbilityTwoUsedAction()
    {
        var val = XPManager.Instance.XP;
        XPUp(val); // or any default value you want to pass

    }
    public void OnTogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            root.Q<VisualElement>("PauseScreen").style.display = DisplayStyle.None;
            GamePlayEvents.GameResumed?.Invoke();
        }
        else
        {
            Time.timeScale = 0f;
            root.Q<VisualElement>("PauseScreen").style.display = DisplayStyle.Flex;
            GamePlayEvents.GamePaused?.Invoke();
        }
        isPaused = !isPaused;
    }
    // public void OnResumeGame()
    // {
    //     Time.timeScale = 1;
    //     var pauseMenu = root.Q<VisualElement>("PauseScreen");
    //     pauseMenu.style.display = DisplayStyle.None;
    //     GameResumed?.Invoke();
    // }
    void QuitGame()
    {
        Time.timeScale = 1f;
        // Load the main menu scene
        SceneManager.LoadSceneAsync(_mainMenuSceneName);
    }
    void RestartGame()
    {
        Time.timeScale = 1f;
        // Load the game scene
        SceneManager.LoadSceneAsync(_gameSceneName);
    }
    void OnGameWon()
    {
        GamePlayEvents.WinScreenShown?.Invoke();
    }
    void OnGameLost()
    {
        GamePlayEvents.LoseScreenShown?.Invoke();
    }
    void OnGameRestarted()
    {
        RestartGame();
    }
    void OnGameQuit()
    {
        QuitGame();
    }
    public void SelectVictoryNextScene()
    {
        // Load the victory scene
        //SceneManager.LoadScene(_victorySceneName);
        
    }
    public void SelectDefeatNextScene()
    {
        // Load the defeat scene
        //SceneManager.LoadScene(_defeatSceneName);
    }
    public void LoadSelectedScene()
    {
        // Load the selected scene
        //SceneManager.LoadScene(_gameSceneName);
    }
}
