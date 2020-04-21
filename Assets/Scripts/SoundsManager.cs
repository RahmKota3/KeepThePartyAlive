using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance;

    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> clips;

    int activeClip = -1;
    
    public void ChangeSong()
    {
        activeClip += 1;

        if (activeClip == clips.Count)
            activeClip = 0;

        audioSource.clip = clips[activeClip];
        audioSource.Play();
    }

    void StartStopMusic()
    {
        if(LevelManager.Instance.CurrentScene != SceneType.Gameplay)
        {
            audioSource.Stop();
        }
        else
        {
            activeClip = Random.Range(0, clips.Count - 1);
            audioSource.clip = clips[activeClip];
            audioSource.Play();
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LevelManager.Instance.OnAfterSceneLoad += StartStopMusic;
    }
}
