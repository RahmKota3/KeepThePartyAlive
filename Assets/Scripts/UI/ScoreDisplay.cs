using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    void DisplayScore(int score)
    {
        scoreText.text = score.ToString();
    }

    void UnsubscribeMethods()
    {
        ScoreManager.Instance.OnScoreChange -= DisplayScore;
        LevelManager.Instance.OnBeforeSceneLoad -= UnsubscribeMethods;
    }

    private void Awake()
    {
        ScoreManager.Instance.OnScoreChange += DisplayScore;
        LevelManager.Instance.OnBeforeSceneLoad += UnsubscribeMethods;
    }
}
