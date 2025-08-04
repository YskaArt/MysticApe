using UnityEngine;
using TMPro;

public class VictoryScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            scoreText.text = $"Puntaje Total: {ScoreManager.Instance.TotalScore}";
        }
    }
}
