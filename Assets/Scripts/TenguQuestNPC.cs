using UnityEngine;
using UnityEngine.SceneManagement;

public class TenguQuestNPC : QuestNPC
{
    [SerializeField] private string sceneToLoad = "Level1";
    private bool interactionInProgress = false;

    public override void Interact()
    {
        if (interactionInProgress)
            return;

        interactionInProgress = true;

        InventorySystem inventory = Object.FindFirstObjectByType<InventorySystem>();

        if (inventory == null)
        {
            Debug.LogError("InventorySystem no encontrado.");
            interactionInProgress = false;
            return;
        }

        if (!questCompleted)
        {
            if (requiresItem && inventory.HasItem(requiredItem))
            {
                questCompleted = true;

                // Mostrar di�logo de �xito y despu�s cambiar escena
                string[] successDialogue = new string[]
                {
                    $"�Perfecto! Tienes el {requiredItem.ItemName}. Puedes avanzar al siguiente nivel."
                };

                DialogueManager.Instance.StartDialogue(npcName, npcIcon, successDialogue, OnDialogueFinished);
            }
            else
            {
                // Mostrar di�logo normal (sin el item)
                base.Interact();
                interactionInProgress = false;
            }
        }
        else
        {
            // Si ya complet� la quest, solo di�logo normal
            DialogueManager.Instance.StartDialogue(npcName, npcIcon, dialogueLines);
            interactionInProgress = false;
        }
    }

    private void OnDialogueFinished()
    {
        // Cambiar escena despu�s de cerrar el di�logo
        SceneManager.LoadScene(sceneToLoad);
        interactionInProgress = false;
    }
}
