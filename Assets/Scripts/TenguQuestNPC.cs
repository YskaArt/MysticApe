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

                // Mostrar diálogo de éxito y después cambiar escena
                string[] successDialogue = new string[]
                {
                    $"¡Perfecto! Tienes el {requiredItem.ItemName}. Puedes avanzar al siguiente nivel."
                };

                DialogueManager.Instance.StartDialogue(npcName, npcIcon, successDialogue, OnDialogueFinished);
            }
            else
            {
                // Mostrar diálogo normal (sin el item)
                base.Interact();
                interactionInProgress = false;
            }
        }
        else
        {
            // Si ya completó la quest, solo diálogo normal
            DialogueManager.Instance.StartDialogue(npcName, npcIcon, dialogueLines);
            interactionInProgress = false;
        }
    }

    private void OnDialogueFinished()
    {
        // Cambiar escena después de cerrar el diálogo
        SceneManager.LoadScene(sceneToLoad);
        interactionInProgress = false;
    }
}
