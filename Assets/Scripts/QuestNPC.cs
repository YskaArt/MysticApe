using UnityEngine;

public class QuestNPC : NPCBase
{
    [Header("Configuración de Quest")]
    [SerializeField] protected bool requiresItem = true;
    [SerializeField] protected ItemData requiredItem;
    [SerializeField] protected ItemData rewardItem;
    [SerializeField] protected bool questCompleted = false;
    protected bool questRewarded = false;

    public override void Interact()
    {
        InventorySystem inventory = Object.FindFirstObjectByType<InventorySystem>();

        if (inventory == null)
        {
            Debug.LogError("InventorySystem no encontrado.");
            return;
        }

        if (!questCompleted)
        {
            if (requiresItem && inventory.HasItem(requiredItem))
            {
                questCompleted = true;
                inventory.AddItem(rewardItem);
                DialogueManager.Instance.ShowMessage($"Gracias por el {requiredItem.ItemName}! Recibiste {rewardItem.ItemName}.");
                questRewarded = true;
            }
            else
            {
                base.Interact(); // diálogo por defecto
            }
        }
        else
        {
            if (!questRewarded && rewardItem != null)
            {
                inventory.AddItem(rewardItem);
                DialogueManager.Instance.ShowMessage($"Recibiste {rewardItem.ItemName}.");
                questRewarded = true;
            }
            else
            {
                DialogueManager.Instance.StartDialogue(npcName, npcIcon, dialogueLines);
            }
        }
    }
}
