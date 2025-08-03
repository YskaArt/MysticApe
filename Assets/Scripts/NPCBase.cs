using UnityEngine;

public class NPCBase : MonoBehaviour, IInteractable
{
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private Sprite npcIcon;
    [SerializeField] private string npcName;

    public virtual void Interact()
    {
        DialogueManager.Instance.StartDialogue(npcName, npcIcon, dialogueLines);
    }
}
