using UnityEngine;

public class NPCBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected string[] dialogueLines;
    [SerializeField] protected Sprite npcIcon;
    [SerializeField] protected string npcName;

    public virtual void Interact()
    {
        DialogueManager.Instance.StartDialogue(npcName, npcIcon, dialogueLines);
    }
}
