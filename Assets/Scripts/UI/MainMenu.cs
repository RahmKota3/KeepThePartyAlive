using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject ControlsObj;
    [SerializeField] TextMeshProUGUI cameraButtonText;

    bool controlsWindowOpen = false;

    [SerializeField] string staticCameraButtonText = "Selected: Static camera (prevents nausea)";
    [SerializeField] string dynamicCameraButtonText = "Selected: Dynamic camera (may cause nausea)";

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

    public void ChangeCamera()
    {
        ReferenceManager.Instance.StaticCamera = !ReferenceManager.Instance.StaticCamera;

        if (ReferenceManager.Instance.StaticCamera)
        {
            cameraButtonText.text = staticCameraButtonText;
        }
        else
        {
            cameraButtonText.text = dynamicCameraButtonText;
        }
    }

    void SetCameraText()
    {
        if (ReferenceManager.Instance.StaticCamera)
        {
            cameraButtonText.text = staticCameraButtonText;
        }
        else
        {
            cameraButtonText.text = dynamicCameraButtonText;
        }
    }

    void UnsubscribeMethods()
    {
        InputManager.Instance.OnPlayButtonPressed -= LoadGameplayScene;
        InputManager.Instance.OnPlayInfiniteButtonPressed -= LoadGameplaySceneInfiniteMode;
        InputManager.Instance.OnControlsButtonPressed -= ShowHideControls;
        InputManager.Instance.OnExitButtonPressed -= ExitGame;
        InputManager.Instance.OnChangeCameraButtonPressed -= ChangeCamera;

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
            InputManager.Instance.OnChangeCameraButtonPressed += ChangeCamera;

            LevelManager.Instance.OnBeforeSceneLoad += UnsubscribeMethods;
        }
        if (LevelManager.Instance.CurrentScene == SceneType.WinScreen || LevelManager.Instance.CurrentScene == SceneType.LoseScreen)
        {
            InputManager.Instance.OnMenuButtonPressed += LoadMainMenuScene;
            InputManager.Instance.OnRestartButtonPressed += LoadGameplayScene;

            LevelManager.Instance.OnBeforeSceneLoad += UnsubscribeMethods;
        }

        SetCameraText();
    }
}
