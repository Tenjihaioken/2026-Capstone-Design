using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public void BuyItem(PlayerStats player, ItemData item)
    {
        if (player == null || item == null)
            return;

        bool success = player.SpendCoin(item.price);

        if (!success)
        {
            Debug.Log("코인이 부족합니다.");
            return;
        }

        ApplyItemEffect(player, item);
        Debug.Log(item.itemName + " 구매 완료");
    }

    private void ApplyItemEffect(PlayerStats player, ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.Heal:
                player.Heal(item.value);
                break;

            case ItemType.AttackBoost:
                player.attackPower += item.value;
                break;

            case ItemType.SpeedBoost:
                player.moveSpeed += item.value;
                break;

            case ItemType.MaxHpBoost:
                player.maxHp += item.value;
                player.currentHp += item.value;
                break;
        }
    }
}