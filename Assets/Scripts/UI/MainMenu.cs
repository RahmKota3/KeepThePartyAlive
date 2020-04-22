using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject ControlsObj;

    bool controlsWindowOpen = false;

    public void LoadGameplayScene()
    {
        TimeManager.Instance.GameplayTimeMinutes = TimeManager.Instance.GameplayTimeInMinutes;
        LevelManager.Instance.LoadScene(SceneType.Gameplay);
    }

    public void LoadGameplaySceneInfiniteMode()
    {
        Debug.Log("menu");
        TimeManager.Instance.GameplayTimeMinutes = Mathf.Infinity;
        LevelManager.Instance.LoadScene(SceneType.Gameplay);
    }

    public void LoadMainMenuScene()
    {
        LevelManager.Instance.LoadScene(SceneType.MainMenu);
    }

    public void ExitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void ShowHideControls()
    {
        controlsWindowOpen = !controlsWindowOpen;

        ControlsObj.SetActive(controlsWindowOpen);
    }

    void UnsubscribeMethods()
    {
        InputManager.Instance.OnPlayButtonPressed -= LoadGameplayScene;
        InputManager.Instance.OnPlayInfiniteButtonPressed -= LoadGameplaySceneInfiniteMode;
        InputManager.Instance.OnControlsButtonPressed -= ShowHideControls;
        InputManager.Instance.OnExitButtonPressed -= ExitGame;

        InputManager.Instance.OnMenuButtonPressed -= LoadMainMenuScene;
        InputManager.Instance.OnRestartButtonPressed -= LoadGameplayScene;

        LevelManager.Instance.OnBeforeSceneLoad -= UnsubscribeMethods;
    }

    private void Start()
    {
        if (LevelManager.Instance.CurrentScene == SceneType.MainMenu)
        {
            InputManager.Instance.OnPlayButtonPressed += LoadGameplayScene;
            InputManager.Instance.OnPlayInfiniteButtonPressed += LoadGameplaySceneInfiniteMode;
            InputManager.Instance.OnControlsButtonPressed += ShowHideControls;
            InputManager.Instance.OnExitButtonPressed += ExitGame;

            LevelManager.Instance.OnBeforeSceneLoad += UnsubscribeMethods;
        }
        if (LevelManager.Instance.CurrentScene == SceneType.WinScreen || LevelManager.Instance.CurrentScene == SceneType.LoseScreen)
        {
            InputManager.Instance.OnMenuButtonPressed += LoadMainMenuScene;
            InputManager.Instance.OnRestartButtonPressed += LoadGameplayScene;

            LevelManager.Instance.OnBeforeSceneLoad += UnsubscribeMethods;
        }
    }
}
