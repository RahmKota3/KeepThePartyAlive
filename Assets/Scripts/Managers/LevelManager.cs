using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType { Gameplay, MainMenu }

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] string gameplaySceneName = "Gameplay";
    [SerializeField] string mainMenuSceneName = "MainMenu";

    public Action OnBeforeSceneLoad;
    public Action OnAfterSceneLoad;

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(SceneType typeToLoad)
    {
        OnBeforeSceneLoad?.Invoke();

        SceneManager.LoadScene(GetSceneName(typeToLoad));
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnAfterSceneLoad?.Invoke();
    }

    string GetSceneName(SceneType type)
    {
        switch (type)
        {
            case SceneType.Gameplay:
                return gameplaySceneName;

            case SceneType.MainMenu:
                return mainMenuSceneName;

            default:
                Debug.LogError("Scene not implemented: " + type);
                return null;
        }
    }

    private void Awake()
    {
        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
}
