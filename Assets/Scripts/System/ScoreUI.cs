using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        UpdateScoreText(ScoreManager.Instance.TotalScore);
        ScoreManager.Instance.OnScoreChanged += UpdateScoreText;
    }

    private void OnDestroy()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreText;
    }

    private void UpdateScoreText(int newScore)
    {
        scoreText.text = $"Score: {newScore}";
    }
}
