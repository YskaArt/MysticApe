using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Animator playerAnimator;
    public static GameOverManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TriggerGameOver()
    {
        StartCoroutine(HandleGameOverSequence());
    }

    private IEnumerator HandleGameOverSequence()
    {
        if (playerAnimator != null)
            playerAnimator.SetBool("IsDeath", true);

        yield return new WaitForSecondsRealtime(1f); 

        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void RetryLevel()
    {
        ScoreManager.Instance.ResetToLevelStart();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
