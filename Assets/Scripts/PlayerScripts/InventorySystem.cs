using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private List<ItemData> items = new();

    public void AddItem(ItemData item)
    {
        if (!items.Contains(item))
            items.Add(item);
    }

    public List<ItemData> GetItems() => items;
    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }
}

