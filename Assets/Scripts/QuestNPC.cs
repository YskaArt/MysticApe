using UnityEngine;
public class QuestNPC : NPCBase
{
    [SerializeField] private ItemData requiredItem;
    [SerializeField] private ItemData rewardItem;
    [SerializeField] private bool questCompleted = false;

    public override void Interact()
    {
        InventorySystem inventory = Object.FindFirstObjectByType<InventorySystem>();

        if (!questCompleted && inventory.HasItem(requiredItem))
        {
            questCompleted = true;
            inventory.AddItem(rewardItem);
            DialogueManager.Instance.ShowMessage($"Gracias por el {requiredItem.ItemName}! Recibiste {rewardItem.ItemName}.");
        }
        else
        {
            base.Interact();
        }
    }
}
