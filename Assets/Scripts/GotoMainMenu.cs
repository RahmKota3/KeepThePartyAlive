using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotoMainMenu : MonoBehaviour
{
    void GotoMain()
    {
        LevelManager.Instance.LoadScene(SceneType.MainMenu);
    }

    void UnsubscribeMethods()
    {
        InputManager.Instance.OnMainMenuButtonPressed -= GotoMain;
        LevelManager.Instance.OnBeforeSceneLoad -= UnsubscribeMethods;
    }

    private void Awake()
    {
        InputManager.Instance.OnMainMenuButtonPressed += GotoMain;
        LevelManager.Instance.OnBeforeSceneLoad += UnsubscribeMethods;
    }
}
