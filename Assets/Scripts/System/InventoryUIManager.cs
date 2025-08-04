using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private RectTransform selector;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    [Header("Slots")]
    [SerializeField] private int totalSlots = 20;

    private List<InventorySlotUI> slots = new();

    public void OpenInventory(List<ItemData> playerItems)
    {
        inventoryPanel.SetActive(true);
        LoadSlots(playerItems);
        ClearSelection();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    private void LoadSlots(List<ItemData> items)
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        slots.Clear();

        for (int i = 0; i < totalSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, gridParent);
            InventorySlotUI slot = slotObj.GetComponent<InventorySlotUI>();

            ItemData data = (i < items.Count) ? items[i] : null;
            slot.Setup(data, this);

            slots.Add(slot);
        }
    }

    public void SelectItem(InventorySlotUI slotUI, ItemData data)
    {
        selector.position = slotUI.GetRectTransform().position;

        itemNameText.text = data.ItemName;
        itemDescriptionText.text = data.Description;
    }

    private void ClearSelection()
    {
        selector.position = Vector3.one * 9999f;
        itemNameText.text = "";
        itemDescriptionText.text = "";
    }
}
