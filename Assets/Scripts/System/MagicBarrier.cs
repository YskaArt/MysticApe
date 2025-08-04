using UnityEngine;

public class MagicBarrier : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData requiredItem;
    [SerializeField] private bool consumeItem = true;
    [SerializeField] private string successMessage = "La barrera m�gica desaparece...";
    [SerializeField] private string failMessage = "Necesitas un objeto m�gico para atravesar esto.";

    public void Interact()
    {
        InventorySystem inventory = FindFirstObjectByType<InventorySystem>();

        if (inventory == null)
        {
            Debug.LogError("InventorySystem no encontrado.");
            return;
        }

        if (inventory.HasItem(requiredItem))
        {
            DialogueManager.Instance.ShowMessage(successMessage);

            if (consumeItem)
            {
                inventory.RemoveItem(requiredItem);
            }

           
            gameObject.SetActive(false);
        }
        else
        {
            DialogueManager.Instance.ShowMessage(failMessage);
        }
    }
}
