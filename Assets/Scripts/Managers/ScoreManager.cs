using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] int scoreForFinishedQuest = 100;
    [SerializeField] int scoreForUnhappyGuest = -200;
    [SerializeField] int scoreForEveryHappyGuest = 50;

    int score = 0;

    public System.Action<int> OnScoreChange;

    public void ResetScore(SceneType type)
    {
        if (type != SceneType.Gameplay)
            return;

        score = 0;
        OnScoreChange?.Invoke(score);
    }

    public void IncreaseScore()
    {
        score += scoreForFinishedQuest;
        OnScoreChange?.Invoke(score);
    }

    public void DecreaseScore()
    {
        score += scoreForUnhappyGuest;
        OnScoreChange?.Invoke(score);
    }

    void IncreaseScoreForEveryHappyGuest(SceneType typeOfScene)
    {
        if (typeOfScene != SceneType.FinalScore)
            return;

        score += NPCManager.Instance.NPCs.Count * scoreForEveryHappyGuest;
        OnScoreChange?.Invoke(score);
    }

    void PushScoreUpdate()
    {
        OnScoreChange?.Invoke(score);
    }

    private void Awake()
    {
        Instance = this;
        LevelManager.Instance.OnBeforeSceneTypeLoaded += ResetScore;
        LevelManager.Instance.OnAfterSceneLoad += PushScoreUpdate;
        LevelManager.Instance.OnBeforeSceneTypeLoaded += IncreaseScoreForEveryHappyGuest;
    }
}
