using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 16;
    [SerializeField] private float invulnerabilityDuration = 1.5f;
    [SerializeField] private Sprite[] heartSprites; // ordenados de 0 (vacío) a 4 (lleno)
    [SerializeField] private Image[] heartImages;   // objetos UI en pantalla (de izquierda a derecha)

    [Header("Feedback Visual")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private float flickerSpeed = 0.1f;

    private int currentHealth;
    private bool isInvulnerable = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHeartsUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHeartsUI();
    }


    private void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            int heartValue = Mathf.Clamp(currentHealth - (i * 4), 0, 4);
            heartImages[i].sprite = heartSprites[heartValue];
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        float elapsed = 0f;
        while (elapsed < invulnerabilityDuration)
        {
            playerSprite.enabled = !playerSprite.enabled;
            yield return new WaitForSeconds(flickerSpeed);
            elapsed += flickerSpeed;
        }

        playerSprite.enabled = true;
        isInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("GAME OVER");
        GameOverManager.Instance.TriggerGameOver();
    }
}
