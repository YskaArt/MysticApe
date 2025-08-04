using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemInside;
    [SerializeField] private Animator animator;
    private bool opened = false;

    public void Interact()
    {
        if (opened) return;

        opened = true;
        animator?.SetTrigger("Open");

        Object.FindFirstObjectByType<InventorySystem>()?.AddItem(itemInside);
        DialogueManager.Instance.ShowMessage($"Obtuviste {itemInside.ItemName}");
    }
}
