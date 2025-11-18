using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static instance of GameManager
    public static GameManager instanceGM;

    

    private Inventory _playerInventory;
    public Inventory PlayerInventory
    {
        get { return _playerInventory; }
        set { _playerInventory = value; }
    }
    // Game states
    public enum GameState { MainMenu, Loading, Gameplay, Victory }
    public GameState currentState;

    // Timer variables
    public float currentGameplayTime = 0.0f;
    public float currentLevelTime = 0.0f;
    public float totalGameplayTime = 0.0f;
    public bool stopTimer = false;

    // Coins variables
    public int currentCoinsCollected = 0;
    public int currentCoinsInLevel = 0;
    public int totalCoinsCollected = 0;
    public int totalCoins = 0;
    public bool countItems = false;

    // Relics variables
    public int currentRelicsCollected = 0;
    public int currentRelicsInLevel = 0;
    public int totalRelicsCollected = 0;
    public int totalRelics = 0;

    public bool showSR = false;
    public bool isPaused = false;

    private void Awake()
    {
        if (instanceGM == null)
        {
            instanceGM = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!showSR)
        {
            UpdateGameState();
        }
        else
        {
            SetVictoryState();
        }

        GameplayTimer();
        GetCurrentItemsInScene();
        GetCurrentItems();
    }

    private void GameplayTimer()
    {
        if (currentState == GameState.Gameplay)
        {
            if (!isPaused)
            {
                currentGameplayTime += Time.deltaTime;
            }
            else
            {
                currentGameplayTime += 0;
            }
        }
        else if (currentState == GameState.Victory && !stopTimer)
        {
            currentLevelTime = currentGameplayTime;
            stopTimer = true;
        }
    }
    public void ResetTimeVariables()
    {
        stopTimer = false;
        currentGameplayTime = 0.0f;
        currentLevelTime = 0.0f;
    }
    private void SetVictoryState()
    {
        Time.timeScale = 0f;
        currentState = GameState.Victory;
    }

    private void GetCurrentItems()
    {
        if (_playerInventory != null)
        {
            currentCoinsCollected = _playerInventory.GetItemCountByID(2);
            currentRelicsCollected = _playerInventory.GetItemCountByID(3);
        }
    }
    private void GetCurrentItemsInScene()
    {
        if (currentState == GameState.Gameplay && !countItems)
        {
            countItems = true;

            int coinsCount = 0;
            int relicsCount = 0;

            ItemObject[] itemsInScene = FindObjectsOfType<ItemObject>();

            foreach (ItemObject item in itemsInScene)
            {
                if (item != null && item.itemData != null)
                {
                    switch (item.itemData.ID)
                    {
                        case 2:
                            coinsCount++;
                            break;
                        case 3:
                            relicsCount++;
                            break;
                    }
                }
            }

            // Assign local counts to global variables
            currentCoinsInLevel = coinsCount;
            currentRelicsInLevel = relicsCount;

            //Debug.Log($"Items in Scene Counted: Coins = {currentCoinsInLevel}, Relics = {currentRelicsInLevel}");
        }
        else if (currentState == GameState.Loading)
        {
            countItems = false; // Prevent multiple calls while counting
        }
    }
    public void ResetCollectableVariables()
    {
        countItems = false;
        currentCoinsCollected = 0;
        currentCoinsInLevel = 0;
        currentRelicsCollected = 0;
        currentRelicsInLevel = 0;  
    }
    public void ResetTotalVariables()
    {
        totalRelicsCollected = 0;
        totalRelics = 0;
        totalCoinsCollected = 0;
        totalCoins = 0;
        totalGameplayTime = 0.0f;
    }

    public void UpdateTotalCounts()
    {
        totalGameplayTime += currentLevelTime;
        totalCoinsCollected += currentCoinsCollected;
        totalRelicsCollected += currentRelicsCollected;
        totalCoins += currentCoinsInLevel;
        totalRelics += currentRelicsInLevel;
    }

    private void UpdateGameState()
    {
        // Check the current scene using Scene_Manager
        string currentSceneName = Scene_Manager.Instance.GetCurrentSceneName();
        //Debug.Log($"{currentSceneName}");

        // Update the game state based on the current scene
        switch (currentSceneName)
        {
            case "MainMenu":
                currentState = GameState.MainMenu;
                ScoreManager.instance.ResetInventory();
                showSR = false;
                countItems = false;
                stopTimer = false;
                ResetTimeVariables();
                ResetCollectableVariables();
                ResetTotalVariables();
                break;
            case "LoadingScreen":
                currentState = GameState.Loading;
                break;
            case "Victory":
                currentState = GameState.Victory;
                break;
            default:
                currentState = GameState.Gameplay;
                break;
        }

        Audio_Manager.Instance.PlayMusicForState(currentState);
    }


    public void ResetTime()
    {
        Time.timeScale = 1f;
        isPaused = false;
        //Debug.LogWarning("Reset");
    }

    public void PauseGame()
    {
        if (!Scene_Manager.Instance.IsSceneExcluded())
        {
            if (!isPaused)
            {
                Time.timeScale = 0f;
                isPaused = true;
                Audio_Manager.Instance.PauseMusic();
                //Debug.Log("Game Paused");
            }
        }
    }

    public void UnpauseGame()
    {
        if (!Scene_Manager.Instance.IsSceneExcluded())
        {
            if (isPaused)
            {
                Time.timeScale = 1f;
                isPaused = false;
                Audio_Manager.Instance.UnpauseMusic();
                //Debug.Log("Game Unpaused");
            }
        }
    }

    public void TogglePause()
    {
        if (!Scene_Manager.Instance.IsSceneExcluded() && !showSR)
        {
            if (isPaused)
            {
                UnpauseGame();
                UI_Manager.instanceUIM.TogglePauseMenu();
            }
            else
            {
                PauseGame();
                UI_Manager.instanceUIM.TogglePauseMenu();
            }
        }
    }
}