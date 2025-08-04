using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    private ItemData itemData;
    private InventoryUIManager inventoryUI;

    public void Setup(ItemData item, InventoryUIManager manager)
    {
        itemData = item;
        inventoryUI = manager;

        iconImage.sprite = item != null ? item.Icon : null;
        iconImage.enabled = item != null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemData != null)
            inventoryUI.SelectItem(this, itemData);
    }

    public RectTransform GetRectTransform() => GetComponent<RectTransform>();
}
