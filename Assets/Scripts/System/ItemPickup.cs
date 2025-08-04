using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;

    [SerializeField] private string pickupMessage = "Obtuviste";

    public void Interact()
    {
        Object.FindFirstObjectByType<InventorySystem>()?.AddItem(itemData);
        DialogueManager.Instance.ShowMessage($"{pickupMessage} {itemData.ItemName}");

        Destroy(gameObject);
    }
}
