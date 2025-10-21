using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Enumeration of scenes in the game. These represent scenes that can be loaded.
/// </summary>
public enum Scenes
{
    MainMenu,
    LoadingScreen,
    Gameplay0,
    Gameplay1,
    Gameplay2,
    Gameplay3,
    Gameplay3B,
    Gameplay3BB_EventQueue,
    Gameplay3C,
    Gameplay4,
    Victory,
}

public static class Scene_Manager
{
    // List of scenes that will be excluded from automatic transitions
    private static List<Scenes> excludedScenes = new List<Scenes>()
    {
        Scenes.MainMenu,
        Scenes.LoadingScreen
    };

    public static Scenes targetScene;

    // A callback action to hold the scene that should be loaded after the loading screen is shown.
    public static Action onLoadCallback;

    /// <summary>
    /// Loads the given scene while first displaying the Loading Screen.
    /// </summary>
    /// <param name="scene">The scene to be loaded, based on the Scenes enum.</param>
    public static void LoadingScene(Scenes scene)
    {
        // Set the target scene before loading the loading screen
        SetTargetScene(scene);

        // Define the onLoadCallback to load the desired scene after the loading screen is shown
        onLoadCallback = () =>
        {
            SceneManager.LoadScene(scene.ToString());
            GameManager.instanceGM.ResetTime();
            onLoadCallback = null;
        };

        // Load the loading screen scene
        SceneManager.LoadScene(Scenes.LoadingScreen.ToString());
    }

    /// <summary>
    /// Callback method that triggers loading the target scene after the loading screen is completed.
    /// This method should be called when the loading screen is finished.
    /// </summary>
    public static void LoadingScreenCallback()
    {
        // Check if the callback is assigned, and if so, execute it to load the target scene.
        if (onLoadCallback != null)
        {
            onLoadCallback();  // Load the target scene
            GameManager.instanceGM.ResetTime();
            onLoadCallback = null;  // Clear the callback to prevent multiple loads
        }
    }

    /// <summary>
    /// Automatically loads the next scene in the Scenes enum order, skipping any scenes that are in the excludedScenes list.
    /// Call this method when you want to change scenes.
    /// </summary>
    public static void LoadNextScene()
    {
        // Convert the Scenes enum into an array to iterate through the scenes.
        Scenes[] sceneArray = (Scenes[])Enum.GetValues(typeof(Scenes));

        // Get the current active scene name from the SceneManager.
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Iterate over the scene array to find the current scene in the enum.
        for (int i = 0; i < sceneArray.Length; i++)
        {
            // If the current scene is found in the enum array
            if (sceneArray[i].ToString() == currentSceneName)
            {
                // Start looking for the next valid scene that is not in the excludedScenes list.
                for (int j = i + 1; j < sceneArray.Length; j++)
                {
                    if (!excludedScenes.Contains(sceneArray[j]))
                    {
                        // If a valid scene is found, load it and exit the method.
                        LoadingScene(sceneArray[j]);
                        if (Inventory.instance != null)
                        {
                            ScoreManager.instance.SaveInventory(Inventory.instance.inventoryDictionary);
                        }
                        return;
                    }
                }

                // If no more valid scenes are found after the current one, loop back to the beginning.
                for (int j = 0; j < sceneArray.Length; j++)
                {
                    if (!excludedScenes.Contains(sceneArray[j]))
                    {
                        // Load the first valid scene and exit the method.
                        LoadingScene(sceneArray[j]);
                        return;
                    }
                }

                Debug.LogWarning("No valid scenes left to load.");
                return;
            }
        }

        Debug.LogError("Current scene not found in Scenes enum.");
    }

    /// <summary>
    /// Method to check if the current scene is in the excluded scenes list.
    /// </summary>
    /// <returns>True if the current scene is excluded, otherwise false.</returns>
    public static bool IsSceneExcluded()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Iterate through the excluded scenes and check if the current scene matches any of them.
        foreach (Scenes scene in excludedScenes)
        {
            if (scene.ToString() == currentSceneName)
            {
                return true; // Current scene is excluded
            }
        }

        return false; // Current scene is not excluded
    }

    /// <summary>
    /// Loads the MainMenu scene.
    /// </summary>
    public static void BackToMainMenu()
    {
        //Scenes scene = Scenes.MainMenu;
        //SetTargetScene(scene);
        GameManager.instanceGM.ResetTime();
        LoadingScene(Scenes.MainMenu);
        GameManager.instanceGM.ResetTimeVariables();
        GameManager.instanceGM.ResetCollectableVariables();
        //Debug.Log("Going back to main menu");
    }

    /// <summary>
    /// Close the application
    /// </summary>
    public static void ExitGame()
    {
        Application.Quit();
        //Debug.Log("Game is closed");
    }

    /// <summary>
    /// Method to reload the current scene
    /// </summary>
    public static void ReloadScene()
    {
        Scenes scene = GetCurrentScene();

        // Set the target scene before loading the loading screen
        SetTargetScene(scene);

        // Define the onLoadCallback to load the desired scene after the loading screen is shown
        onLoadCallback = () =>
        {
            SceneManager.LoadScene(scene.ToString());
            GameManager.instanceGM.ResetTime();
            GameManager.instanceGM.ResetTimeVariables();
            GameManager.instanceGM.ResetCollectableVariables();
            onLoadCallback = null;
        };

        // Load the loading screen scene
        SceneManager.LoadScene(Scenes.LoadingScreen.ToString());
    }

    /// <summary>
    /// Method to return the current scene as a Scenes enum.
    /// </summary>
    public static Scenes GetCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Try to match the current scene name with the Scenes enum
        foreach (Scenes scene in Enum.GetValues(typeof(Scenes)))
        {
            if (scene.ToString() == currentSceneName)
            {
                return scene;
            }
        }

        Debug.LogError("Current scene not found in Scenes enum.");
        return Scenes.MainMenu; // Default to MainMenu if not found (can adjust if needed)
    }

    // Method to set the target scene to be loaded
    public static void SetTargetScene(Scenes scene)
    {
        targetScene = scene;
    }

    // Method to get the target scene
    public static Scenes GetTargetScene()
    {
        return targetScene;
    }
}