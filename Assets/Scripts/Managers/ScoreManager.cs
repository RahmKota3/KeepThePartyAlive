using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] int scoreForFinishedQuest = 100;
    [SerializeField] int scoreForUnhappyGuest = -200;
    [SerializeField] int scoreForEveryHappyGuest = 50;

    public int Score = 0;

    public int AmountOfNpcs = 0;

    public System.Action<int> OnScoreChange;

    public void ResetScore(SceneType type)
    {
        if (type != SceneType.Gameplay)
            return;

        Score = 0;
        OnScoreChange?.Invoke(Score);
    }

    public void IncreaseScore()
    {
        Score += scoreForFinishedQuest;
        OnScoreChange?.Invoke(Score);
    }

    public void DecreaseScore()
    {
        Score += scoreForUnhappyGuest;
        OnScoreChange?.Invoke(Score);
    }

    //void IncreaseScoreForEveryHappyGuest(SceneType typeOfScene)
    //{
    //    if (typeOfScene != SceneType.WinScreen && typeOfScene != SceneType.LoseScreen)
    //        return;

    //    Debug.Log(AmountOfNpcs * scoreForEveryHappyGuest);

    //    Score += (AmountOfNpcs * scoreForEveryHappyGuest);
    //    OnScoreChange?.Invoke(Score);

    //    SaveHighScore();
    //}

    void PushScoreUpdate()
    {
        OnScoreChange?.Invoke(Score);
    }

    void SaveHighScore(SceneType typeOfScene)
    {   
        if (typeOfScene != SceneType.WinScreen && typeOfScene != SceneType.LoseScreen)
            return;

        if(PlayerPrefs.HasKey(GlobalVariables.HighScorePrefsName) == false || 
            Score > PlayerPrefs.GetInt(GlobalVariables.HighScorePrefsName))
            PlayerPrefs.SetInt(GlobalVariables.HighScorePrefsName, Score);
    }

    private void Awake()
    {
        Instance = this;
        LevelManager.Instance.OnBeforeSceneTypeLoaded += ResetScore;
        LevelManager.Instance.OnAfterSceneLoad += PushScoreUpdate;
        LevelManager.Instance.OnBeforeSceneTypeLoaded += SaveHighScore;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
            PlayerPrefs.DeleteKey(GlobalVariables.HighScorePrefsName);
    }
}
