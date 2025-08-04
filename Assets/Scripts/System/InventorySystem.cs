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

    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }

   
    public void RemoveItem(ItemData item)
    {
        if (items.Contains(item))
            items.Remove(item);
    }

    
    public List<ItemData> GetItems()
    {
        return new List<ItemData>(items);
    }
}
