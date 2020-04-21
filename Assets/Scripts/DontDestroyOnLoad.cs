using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        LevelManager.Instance.LoadScene(SceneType.MainMenu);
    }
}
