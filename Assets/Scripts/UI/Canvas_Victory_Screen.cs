using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Canvas_Victory_Screen : MonoBehaviour
{
    [SerializeField] private TMP_Text totalTimeText;
    [SerializeField] private TMP_Text totalCoinsText;
    [SerializeField] private TMP_Text totalRelicsText;

    private void Update()
    {
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        float currentLevelTime = GameManager.instanceGM.totalGameplayTime;

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(currentLevelTime / 60);
        int seconds = Mathf.FloorToInt(currentLevelTime % 60);

        // Format the time as MM:SS
        totalTimeText.text = $"{minutes:00}:{seconds:00}";


        int totalCoins = GameManager.instanceGM.totalCoins;
        int totalCoinsCollected = GameManager.instanceGM.totalCoinsCollected;

        totalCoinsText.text = $"{totalCoinsCollected:00} / {totalCoins:00}";

        int totalRelics = GameManager.instanceGM.totalRelics;
        int totalRelicsCollected = GameManager.instanceGM.totalRelicsCollected;

        totalRelicsText.text = $"{totalRelicsCollected:00} / {totalRelics:00}";
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        GameManager.instanceGM.showSR = false;
        GameManager.instanceGM.countItems = false;
        GameManager.instanceGM.stopTimer = false;
        GameManager.instanceGM.ResetTimeVariables();
        GameManager.instanceGM.ResetCollectableVariables();
        GameManager.instanceGM.ResetTotalVariables();
        Scene_Manager.BackToMainMenu();
    }
}