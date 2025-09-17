using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Canvas_SR_behaviour : MonoBehaviour
{
    public Canvas srCanvas;

    public TMP_Text timeText;
    public TMP_Text coinsText;
    public TMP_Text relicsText;

    private void Update()
    {
        ShowScreenResults();
    }

    private void ShowScreenResults()
    {
        if (Scene_Manager.GetCurrentScene() != Scenes.Victory)
        {
            if (GameManager.instanceGM.currentState == GameManager.GameState.Victory)
            {
                srCanvas.gameObject.SetActive(true);
                UpdateTime();
                UpdateCoins();
                UpdateRelics();
            }
            else
            {
                srCanvas.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateTime()
    {
        float currentLevelTime = GameManager.instanceGM.currentLevelTime;

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(currentLevelTime / 60);
        int seconds = Mathf.FloorToInt(currentLevelTime % 60);

        // Format the time as MM:SS
        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateCoins()
    {
        int currentCoinsInLevel = GameManager.instanceGM.currentCoinsInLevel;
        int currentCoinsCollected = GameManager.instanceGM.currentCoinsCollected;

        coinsText.text = $"{currentCoinsCollected:00} / {currentCoinsInLevel:00}";
    }

    private void UpdateRelics()
    {
        int currentRelicsInLevel = GameManager.instanceGM.currentRelicsInLevel;
        int currentRelicsCollected = GameManager.instanceGM.currentRelicsCollected;

        relicsText.text = $"{currentRelicsCollected:00} / {currentRelicsInLevel:00}";
    }


    public void NextLevelButton()
    {
        Time.timeScale = 1f;
        GameManager.instanceGM.showSR = false;
        GameManager.instanceGM.countItems = false;
        GameManager.instanceGM.UpdateTotalCounts();
        GameManager.instanceGM.ResetTimeVariables();
        GameManager.instanceGM.ResetCollectableVariables();
        GameManager.instanceGM.currentState = GameManager.GameState.Gameplay;
        Scene_Manager.LoadNextScene();
    }
    public void RestartButton()
    {
        Time.timeScale = 1f;
        GameManager.instanceGM.showSR = false;
        GameManager.instanceGM.countItems = false;
        GameManager.instanceGM.ResetTimeVariables();
        GameManager.instanceGM.ResetCollectableVariables();
        GameManager.instanceGM.currentState = GameManager.GameState.Gameplay;
        Scene_Manager.ReloadScene();
    }
    public void BackToMainMenuButton()
    {
        Time.timeScale = 1f;
        GameManager.instanceGM.showSR = false;
        GameManager.instanceGM.countItems = false;
        GameManager.instanceGM.ResetTimeVariables();
        GameManager.instanceGM.ResetCollectableVariables();
        GameManager.instanceGM.currentState = GameManager.GameState.MainMenu;
        Scene_Manager.BackToMainMenu();
    }
}