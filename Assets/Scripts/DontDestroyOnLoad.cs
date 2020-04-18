using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        // TODO: Change this after main menu is added.
        LevelManager.Instance.LoadScene(SceneType.Gameplay);
    }
}
