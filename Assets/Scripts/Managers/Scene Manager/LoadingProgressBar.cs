using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LoadingProgressBar : MonoBehaviour
{
    [SerializeField] private Image circularProgressBar;
    [SerializeField] private float minimumLoadingTime = 2f;

    private void Start()
    {
        if (circularProgressBar != null)
            circularProgressBar.fillAmount = 0f;

        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        Debug.Log("Operation beginning");

        int targetSceneIndex = Scene_Manager.Instance.TargetSceneIndex;
        string sceneName = Scene_Manager.Instance.GetSceneName(targetSceneIndex);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            float easedValue = Mathf.Lerp(circularProgressBar.fillAmount, progress, 3f * Time.deltaTime);
            circularProgressBar.fillAmount = easedValue;

            elapsedTime += Time.deltaTime;
            Debug.Log($"Operation progress: {progress:F2}, elapsed: {elapsedTime:F2}");

            if (operation.progress >= 0.9f && elapsedTime >= minimumLoadingTime)
            {
                Debug.Log("Ready to activate next scene...");
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        Debug.Log("Async operation finished!"); 
    }
}