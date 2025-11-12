using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public static Scene_Manager Instance { get; private set; }

    private ABB sceneTree = new ABB();
    private List<int> excludedIndixes = new List<int>();
    private Dictionary<int, string> sceneNames = new Dictionary<int, string>();

    public int TargetSceneIndex { get; private set; }
    public int currentSceneIndex = 0;

    public Action onLoadCallback;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeSceneTree();
    }

    public void InitializeSceneTree()
    {
        sceneTree.Inicialize();
        sceneNames.Clear();
        excludedIndixes.Clear();

        int count = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = Path.GetFileNameWithoutExtension(path);
            sceneTree.Add(i);
            sceneNames[i] = name;

            if (name == "MainMenu" || name == "LoadingScreen")
                excludedIndixes.Add(i);
        }

        //Debug.Log($"Scene tree initialized with {count} scenes.");
        //sceneTree.DisplayTree(TreeOrderTypes.InOrder);
    }

    public void LoadNextScene()
    {
        currentSceneIndex = GetCurrentSceneIndex();

        int nextIndex = FindNextSceneIndex(currentSceneIndex);

        if (nextIndex == -1)
        {
            Debug.Log("No next scene found, reloading first available gameplay scene.");
            nextIndex = GetFirstPlayableScene();
        }

        LoadSceneWithLoadingScreen(nextIndex);
    }

    private int FindNextSceneIndex(int currentIndex)
    {
        List<int> orderedScenes = sceneTree.GetInOrderList();

        int idx = orderedScenes.IndexOf(currentIndex);
        if (idx == -1) return -1;

        for (int i = idx + 1; i < orderedScenes.Count; i++)
        {
            if (!excludedIndixes.Contains(orderedScenes[i]))
                return orderedScenes[i];
        }
        return -1;
    }

    private List<int> GetSceneIndixesInOrder()
    {
        List<int> result = new List<int>();
        sceneTree.GetInOrderList();
        return result;
    }

    public void ReloadCurrentScene()
    {
        int currentIndex = GetCurrentSceneIndex();
        LoadSceneWithLoadingScreen(currentIndex);
    }

    public void LoadMainMenu()
    {
        foreach (var kvp in sceneNames)
        {
            if (kvp.Value == "MainMenu")
            {
                LoadSceneWithLoadingScreen(kvp.Key);
                return;
            }
        }
        Debug.LogWarning("MainMenu not found in build settings!");
    }

    public void LoadSceneByIndex(int index)
    {
        NodeABB found = sceneTree.Search(sceneTree.root, index);
        if (found == null)
        {
            Debug.LogError($"Scene index {index} not found in ABB!");
            return;
        }

        if (!sceneNames.TryGetValue(index, out string name))
        {
            Debug.LogError($"Scene name for index {index} not found!");
            return;
        }

        Debug.Log($"Loading scene: {name}");
        SceneManager.LoadScene(name);
        GameManager.instanceGM.ResetTime();
    }

    public void LoadSceneWithLoadingScreen(int targetSceneIndex)
    {
        if (GetCurrentSceneName() == "LoadingScreen")
            return;

        NodeABB found = sceneTree.Search(sceneTree.root, targetSceneIndex);
        if (found == null)
        {
            Debug.LogError($"Scene index {targetSceneIndex} not found in ABB!");
            return;
        }

        GameManager.instanceGM.ResetTime();
        TargetSceneIndex = targetSceneIndex;

        if (sceneNames.Values.Contains("LoadingScreen"))
        {
            Debug.Log($"Loading LoadingScreen before {sceneNames[targetSceneIndex]}...");
            SceneManager.LoadScene("LoadingScreen");
        }
        else
        {
            Debug.LogWarning("LoadingScreen not found. Loading target scene directly.");
            LoadSceneByIndex(targetSceneIndex);
        }
    }

    public void LoadingScreenCallback()
    {
        NodeABB found = sceneTree.Search(sceneTree.root, TargetSceneIndex);
        if (found == null)
        {
            Debug.LogError("No valid TargetSceneIndex found in ABB!");
            return;
        }

        if (sceneNames.TryGetValue(TargetSceneIndex, out string targetName))
        {
            Debug.Log($"[LoadingScreen] Now loading target scene: {targetName}");
            SceneManager.LoadScene(targetName);
            GameManager.instanceGM.ResetTime();
        }
        else
        {
            Debug.LogError("No valid TargetSceneIndex name found!");
        }
    }

    // Help functions
    public int GetFirstPlayableScene()
    {
        List<int> orderedScenes = GetSceneIndixesInOrder();

        foreach (int idx in orderedScenes)
            if (!excludedIndixes.Contains(idx))
                return idx;

        return -1;
    }

    public string GetSceneName(int index)
    {
        if (sceneNames.TryGetValue(index, out string name))
            return name;

        NodeABB found = sceneTree.Search(sceneTree.root, index);
        return found != null ? index.ToString() : string.Empty;
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public int GetCurrentSceneIndex()
    {
        return sceneTree.Search(currentSceneIndex).info;
    }

    public bool IsSceneExcluded()
    {
        int currentIndex = GetCurrentSceneIndex();
        return excludedIndixes.Contains(currentIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}