using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public event Action<int> OnScoreChanged;

    private int currentScore = 0;
    private int levelStartScore = 0;

    public int TotalScore => currentScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void SaveLevelStartScore()
    {
        levelStartScore = currentScore;
    }

    public void ResetToLevelStart()
    {
        currentScore = levelStartScore;
        OnScoreChanged?.Invoke(currentScore);
    }
}
