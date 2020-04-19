using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

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

    private void Awake()
    {
        Instance = this;
    }
}
