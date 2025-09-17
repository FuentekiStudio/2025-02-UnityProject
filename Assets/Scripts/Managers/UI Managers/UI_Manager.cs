using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instanceUIM;

    [SerializeField] private UI_Manager_SFX_component sfx_Component;

    // Menu in-game panel
    [Header("Menu In-Game HUD")]
    [SerializeField] private GameObject panelPause;
    [SerializeField] private GameObject optionsMenuContainer;
    [SerializeField] private GameObject background;


    private void Awake()
    {
        if (instanceUIM != null && instanceUIM != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instanceUIM = this;
        }
    }

    private void Update()
    {
        if (Scene_Manager.IsSceneExcluded())
        {
            panelPause.SetActive(false);
            background.SetActive(false);
        }
    }


    public void TogglePauseMenu()
    {
        if (!Scene_Manager.IsSceneExcluded())
        {
            if (!GameManager.instanceGM.isPaused)
            {
                sfx_Component.PlayToggleMenuSFX();
                panelPause.SetActive(false);
                background.SetActive(false);
                optionsMenuContainer.SetActive(false);
            }
            else
            {
                sfx_Component.PlayToggleMenuSFX();
                panelPause.SetActive(true);
                background.SetActive(true);
            }
        }
    }

    //Methods for button callbacks
    public void ContinueGame()
    {
        GameManager.instanceGM.UnpauseGame();
        TogglePauseMenu();
    }
    public void ResetButton()
    {
        panelPause.SetActive(false);
        optionsMenuContainer.SetActive(false);
        Scene_Manager.ReloadScene();
        GameManager.instanceGM.ResetTime();
        GameManager.instanceGM.ResetTimeVariables();
        GameManager.instanceGM.ResetCollectableVariables();
    }
    public void MainMenuButtonCallback()
    {
        Scene_Manager.BackToMainMenu();
    }
    public void OptionButtonCallback()
    {
        sfx_Component.PlayToggleMenuSFX();
        panelPause.SetActive(false);
        optionsMenuContainer.SetActive(true);
    }
    public void BackButtonCallback()
    {
        sfx_Component.PlayToggleMenuSFX();
        panelPause.SetActive(true);
        optionsMenuContainer.SetActive(false);
    }

    public void CloseGame()
    {
        Scene_Manager.ExitGame();
    }
}
