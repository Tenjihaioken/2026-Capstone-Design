using UnityEngine;

public enum ItemType
{
    Heal,
    AttackBoost,
    SpeedBoost,
    MaxHpBoost
}

[CreateAssetMenu(fileName = "NewItemData", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int price;
    [TextArea] public string description;

    public ItemType itemType;
    public int value;
}