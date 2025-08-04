using UnityEngine;

public class VendorNPC : NPCBase
{
    public override void Interact()
    {
        
        InventorySystem inventory = FindFirstObjectByType<InventorySystem>();
        if (inventory == null)
        {
            Debug.LogError("InventorySystem no encontrado.");
            return;
        }

        int totalValue = 0;
        var items = inventory.GetItems();

        foreach (ItemData item in items)
        {
            if (item.isSellable)
            {
                totalValue += item.sellValue;
                inventory.RemoveItem(item);
            }
        }

        if (totalValue > 0)
        {
            ScoreManager.Instance.AddScore(totalValue);
            DialogueManager.Instance.ShowMessage($"¡Gracias! Vendiste tus objetos por {totalValue} puntos.");
        }
        else
        {
            DialogueManager.Instance.ShowMessage("No tienes nada que pueda comprarte.");
        }
    }
}
