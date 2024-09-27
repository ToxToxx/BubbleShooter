using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        LoadingScreen,
        MainMenu, 
        GameScene,
    }

    private static Scene _targetScene;

    public static void Load(Scene targetScene)
    {
        try
        {
            _targetScene = targetScene;
            Debug.Log("Starting loading of " + targetScene.ToString()); 

            SceneManager.LoadScene(Scene.LoadingScreen.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load the loading screen: " + ex.Message);
        }
    }

    public static void LoaderCallback()
    {
        try
        {
            Debug.Log("Loading resources for " + _targetScene.ToString());

            SceneManager.LoadScene(_targetScene.ToString());

            Debug.Log(_targetScene.ToString() + " loaded successfully!"); 
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load the scene " + _targetScene.ToString() + ": " + ex.Message);
        }
    }
}
