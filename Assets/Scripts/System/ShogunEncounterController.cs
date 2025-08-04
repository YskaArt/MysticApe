using UnityEngine;
using System.Collections;

public class ShogunEncounterController : MonoBehaviour
{
    [Header("Referencias del jefe")]
    [SerializeField] private ShogunBoss shogunScript;
    [SerializeField] private GameObject BossHealth;

    [Header("UI y diálogo")]
    [SerializeField] private string bossName = "SHOGUN";
    [TextArea(2, 5)] public string[] startDialogue;
    [TextArea(2, 5)] public string[] endDialogue;
    [SerializeField] private Sprite bossIcon;

    [Header("Control")]
    [SerializeField] private bool triggerOnce = true;

    private bool alreadyTriggered = false;
    private bool bossDefeated = false;

    private void Start()
    {
        if (shogunScript != null)
            shogunScript.enabled = false;
        BossHealth.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyTriggered && triggerOnce) return;

        if (other.CompareTag("Player"))
        {
            alreadyTriggered = true;
            DialogueManager.Instance.StartDialogue(bossName, bossIcon, startDialogue, OnStartDialogueFinished);
        }
    }

    private void OnStartDialogueFinished()
    {
        if (shogunScript != null)
        {
            BossHealth.SetActive(true);
            shogunScript.enabled = true; 
            BossHealthUI.Instance.SetMaxHealth(shogunScript.MaxHealth);
        }

        shogunScript.OnBossDefeated += OnBossDefeated;
    }

    private void OnBossDefeated()
    {
        if (bossDefeated) return;
        bossDefeated = true;

        DialogueManager.Instance.StartDialogue(bossName, bossIcon, endDialogue, () =>
        {
            StartCoroutine(DelayedVictory());
        });
    }

    private IEnumerator DelayedVictory()
    {
        yield return new WaitForSeconds(3.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("VictoryPanel");
    }
}
