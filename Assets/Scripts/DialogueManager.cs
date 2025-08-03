using UnityEngine;
using System.Collections;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
    [SerializeField] private TMPro.TextMeshProUGUI npcNameText;
    [SerializeField] private UnityEngine.UI.Image npcIcon;
    public bool IsDialogueActive => dialoguePanel.activeSelf;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowMessage(string message)
    {
        StartDialogue("", null, new string[] { message });
        dialoguePanel.SetActive(true);
        dialogueText.text = message;
        npcNameText.text = "";
        npcIcon.sprite = null;
    }

    public void StartDialogue(string name, Sprite icon, string[] lines)
    {
        if (PauseMenuManager.Instance != null)
            PauseMenuManager.Instance.ForceClosePause();

        dialoguePanel.SetActive(true);
        npcNameText.text = name;
        npcIcon.sprite = icon;
        StartCoroutine(DialogueCoroutine(lines));
    }

    private IEnumerator DialogueCoroutine(string[] lines)
    {
        foreach (string line in lines)
        {
            dialogueText.text = line;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        }

        dialoguePanel.SetActive(false);
    }
}
