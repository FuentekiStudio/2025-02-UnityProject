using UnityEngine;

public class Back_To_Main_Menu : MonoBehaviour
{
    public void BackToMainMenu()
    {
        Scene_Manager.Instance.LoadMainMenu();
    }
}