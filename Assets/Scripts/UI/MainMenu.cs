using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject ControlsObj;

    public void LoadGameplayScene()
    {
        LevelManager.Instance.LoadScene(SceneType.Gameplay);
    }

    public void ExitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void ShowHideControls(bool show)
    {
        ControlsObj.SetActive(show);
    }
}
