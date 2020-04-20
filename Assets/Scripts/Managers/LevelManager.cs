using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType { Gameplay, MainMenu, WinScreen, LoseScreen }

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] string gameplaySceneName = "Gameplay";
    [SerializeField] string mainMenuSceneName = "MainMenu";
    [SerializeField] string winSceneName = "WinScreen";
    [SerializeField] string lossSceneName = "LossScreen";

    public Action OnBeforeSceneLoad;
    public Action<SceneType> OnBeforeSceneTypeLoaded;
    public Action OnAfterSceneLoad;

    public SceneType CurrentScene = SceneType.MainMenu;

    public void LoadWinLevel()
    {
        LoadScene(SceneType.WinScreen);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(SceneType typeToLoad)
    {
        if (TimeManager.Instance != null && TimeManager.Instance.gameObject != null)
            TimeManager.Instance.ResetCoroutines();

        CurrentScene = typeToLoad;
        Debug.Log(CurrentScene);

        OnBeforeSceneLoad?.Invoke();
        OnBeforeSceneTypeLoaded?.Invoke(typeToLoad);

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

            case SceneType.WinScreen:
                return winSceneName;

            case SceneType.LoseScreen:
                return lossSceneName;

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

    private void Start()
    {
        InputManager.Instance.OnRestartButtonPressed += RestartLevel;
    }
}
