using UnityEngine;

public class Back_to_menu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        Scene_Manager.Instance.LoadMainMenu();
    }
}