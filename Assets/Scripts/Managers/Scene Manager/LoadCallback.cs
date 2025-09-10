using UnityEngine;

public class LoadCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Scene_Manager.LoadingScreenCallback();
        }
    }
}