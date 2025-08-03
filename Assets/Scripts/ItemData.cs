using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;
}
