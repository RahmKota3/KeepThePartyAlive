using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI bestScore;

    [SerializeField] GameObject newBestObj;

    void DisplayScore()
    {
        score.text = ScoreManager.Instance.Score.ToString();
        bestScore.text = PlayerPrefs.GetInt(GlobalVariables.HighScorePrefsName).ToString();

        if (ScoreManager.Instance.Score >= PlayerPrefs.GetInt(GlobalVariables.HighScorePrefsName))
        {
            newBestObj.SetActive(true);
        }
    }

    private void Start()
    {
        DisplayScore();
    }
}
