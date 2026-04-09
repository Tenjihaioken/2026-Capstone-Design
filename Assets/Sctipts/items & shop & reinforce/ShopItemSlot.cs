using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemSlot : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private ItemData currentItem;
    private ShopManager shopManager;
    private PlayerStats player;

    public void Setup(ItemData item, ShopManager manager, PlayerStats targetPlayer)
    {
        currentItem = item;
        shopManager = manager;
        player = targetPlayer;

        itemNameText.text = item.itemName;
        priceText.text = item.price.ToString();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyCurrentItem);
    }

    private void BuyCurrentItem()
    {
        shopManager.BuyItem(player, currentItem);
    }
}