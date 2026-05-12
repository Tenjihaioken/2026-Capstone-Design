using UnityEngine;

public enum ShopItemType
{
    Heal,
    AttackUp,
    MoveSpeedUp,
    MaxHpUp
}

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Game/Shop Item")]
public class ShopItemData : ScriptableObject
{
    public string itemName;
    public int price;
    [TextArea] public string description;

    public ShopItemType itemType;
    public int intValue;
    public float floatValue;
}