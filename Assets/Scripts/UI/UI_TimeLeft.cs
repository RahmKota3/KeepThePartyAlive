using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_TimeLeft : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeLeftText;

    float GetMinutes(float time)
    {
        return Mathf.Floor(time / 60);
    }

    float GetSecondsWithoutMinutes(float time)
    {
        return Mathf.Floor(time - (GetMinutes(time) * 60));
    }

    IEnumerator UpdateTimeTextCoroutine()
    {
        float timeLeftSeconds = (TimeManager.Instance.GameplayTimeMinutes * 60) - TimeManager.Instance.GameTime;
        timeLeftText.text = "Time left: " + GetMinutes(timeLeftSeconds) + ":" + GetSecondsWithoutMinutes(timeLeftSeconds).ToString("##");

        yield return new WaitForSeconds(1);

        StartCoroutine(UpdateTimeTextCoroutine());
    }

    private void Start()
    {
        StartCoroutine(UpdateTimeTextCoroutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
