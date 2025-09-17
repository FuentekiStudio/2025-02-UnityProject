using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LoadingProgressBar : MonoBehaviour
{
    [SerializeField] private Image circularProgressBar;
    [SerializeField] private float minimumLoadingTime = 2f; // Minimum time to show the loading screen

    private void Start()
    {
        // Ensure the fill amount starts at 0
        if (circularProgressBar != null)
            circularProgressBar.fillAmount = 0f;

        // Start the loading coroutine
        StartCoroutine(LoadSceneAsync());
    }

    /// <summary>
    /// Coroutine to load the target scene asynchronously and update the progress bar.
    /// </summary>
    private IEnumerator LoadSceneAsync()
    {
        Scenes sceneToLoad = Scene_Manager.GetTargetScene();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad.ToString());
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        // Update the progress bar based on the loading progress
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            float easedValue = Mathf.Lerp(circularProgressBar.fillAmount, progress*1.1f, minimumLoadingTime*Time.deltaTime);
            circularProgressBar.fillAmount = easedValue;

            // Track the elapsed time
            elapsedTime += Time.deltaTime;

            // If the operation is complete and the minimum loading time has passed, activate the scene
            if (operation.progress >= 0.9f && elapsedTime >= minimumLoadingTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (operation.isDone)
        {
            Scene_Manager.LoadingScreenCallback();
        }       
    }
}