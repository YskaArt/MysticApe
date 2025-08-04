using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public static BossHealthUI Instance;

    [SerializeField] private GameObject bossHealthPanel;
    [SerializeField] private Image fillBar;

    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetMaxHealth(int max)
    {
        
        fillBar.fillAmount = 1f;
        bossHealthPanel.SetActive(true);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        fillBar.fillAmount = (float)currentHealth / maxHealth;
    }

    public void Hide()
    {
        bossHealthPanel.SetActive(false);
    }
    
}