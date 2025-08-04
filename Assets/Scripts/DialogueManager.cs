using UnityEngine;
using System;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueText;
    [SerializeField] private TMPro.TextMeshProUGUI npcNameText;
    [SerializeField] private UnityEngine.UI.Image npcIcon;

    public bool IsDialogueActive => dialoguePanel.activeSelf;

    private Action onDialogueFinishedCallback; // Nuevo callback

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowMessage(string message)
    {
        StartDialogue("", null, new string[] { message });
        dialogueText.text = message;
        npcNameText.text = "";
        npcIcon.sprite = null;
    }

    public void StartDialogue(string name, Sprite icon, string[] lines)
    {
        StartDialogue(name, icon, lines, null); // Llama a la sobrecarga con null
    }

    
    public void StartDialogue(string name, Sprite icon, string[] lines, Action onFinished)
    {
        if (PauseMenuManager.Instance != null)
            PauseMenuManager.Instance.ForceClosePause();

        onDialogueFinishedCallback = onFinished;

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

        // Ejecutar el callback si existe
        onDialogueFinishedCallback?.Invoke();
        onDialogueFinishedCallback = null;
    }
}
