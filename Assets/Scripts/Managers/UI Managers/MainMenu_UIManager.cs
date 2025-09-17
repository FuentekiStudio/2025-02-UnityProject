using UnityEngine;
using TMPro;

public class MainMenu_UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuContainer;
    [SerializeField] private GameObject optionsMenuContainer;


    private void Start()
    {
        mainMenuContainer.SetActive(true);
        optionsMenuContainer.SetActive(false);
    }

    public void PlayButtonCallback()
    {
        Scene_Manager.LoadNextScene();
    }

    public void OptionButtonCallback()
    {
        mainMenuContainer.SetActive(false);
        optionsMenuContainer.SetActive(true);
    }

    public void BackButtonCallback()
    {
        mainMenuContainer.SetActive(true);
        optionsMenuContainer.SetActive(false);
    }

    public void ExitButtonCallback()
    {
        Scene_Manager.ExitGame();
    }
}