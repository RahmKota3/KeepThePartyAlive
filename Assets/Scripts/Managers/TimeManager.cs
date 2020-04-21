﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [SerializeField] int gameplayTimeMinutes = 5;

    public float GameTime = 0;

    public void StartTimer(float time, System.Action functionToCall)
    {
        StartCoroutine(StartTimerCoroutine(time, functionToCall));
    }

    public void StartQuestFailTimer(float time, System.Action<Quest> functionToCall, Quest questToFail, out Coroutine coroutine)
    {
        coroutine = StartCoroutine(StartTimerCoroutine(time, functionToCall, questToFail));
    }

    IEnumerator StartTimerCoroutine(float time, System.Action functionToCall)
    {
        yield return new WaitForSeconds(time);

        functionToCall?.Invoke();
    }

    IEnumerator StartTimerCoroutine(float time, System.Action<Quest> functionToCall, Quest questToFail)
    {
        yield return new WaitForSeconds(time);

        functionToCall?.Invoke(questToFail);
    }

    public void ResetCoroutines()
    {
        StopAllCoroutines();
    }

    void EndGame()
    {
        StopAllCoroutines();
        LevelManager.Instance.LoadScene(SceneType.WinScreen);
    }
    
    private void Awake()
    {
        Instance = this;

        //StartTimer(5, EndGame);
        //StartTimer(gameplayTimeMinutes * 60, EndGame);
    }

    private void Update()
    {
        if (LevelManager.Instance.CurrentScene != SceneType.Gameplay || Application.isPlaying == false)
        {
            GameTime = 0;
            return;
        }

        GameTime += Time.deltaTime;

        if (GameTime >= gameplayTimeMinutes * 60)
            EndGame();
    }
}
