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
        currentSceneIndex = GetCurrentSceneIndex();
    }

    private void Update()
    {

        currentSceneIndex = GetCurrentSceneIndex();
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
            sceneTree.Add(i, name);
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

    public void LoadSceneByIndex(int index)
    {
        NodeABB found = sceneTree.Search(sceneTree.root, index);
        if (found == null)
        {
            Debug.LogError($"Scene index {index} not found in ABB!");
            return;
        }

        Debug.Log($"Loading scene: {found.id}");
        SceneManager.LoadScene(found.info);
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

        Debug.Log($"[LoadingScreen] Now loading target scene: {found.id}");
        
        SceneManager.LoadScene(found.info);
        GameManager.instanceGM.ResetTime();
    }

    // Help functions
    private int FindNextSceneIndex(int currentIndex)
    {
        int nextIndex = currentIndex + 1;

        for (int i = nextIndex; i <= sceneTree.Greatest(sceneTree.root); i++)
        {
            if (!excludedIndixes.Contains(sceneTree.Search(i).info))
                return sceneTree.Search(i).info;
        }
        return -1;
    }

    private List<int> GetSceneIndixesInOrder()
    {
        List<int> result = new List<int>();
        sceneTree.GetInOrderList();
        return result;
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

    public void ReloadCurrentScene()
    {
        int currentIndex = GetCurrentSceneIndex();
        LoadSceneWithLoadingScreen(currentIndex);
    }

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
        NodeABB found = sceneTree.Search(sceneTree.root, index);
        return found != null ? found.id : string.Empty;
    }

    public string GetCurrentSceneName()
    {
        //return sceneTree.Search(currentSceneIndex).id;
        return SceneManager.GetActiveScene().name;
    }

    public int GetCurrentSceneIndex()
    {
        //return sceneTree.Search(currentSceneIndex).info;
        return SceneManager.GetActiveScene().buildIndex;
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