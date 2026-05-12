using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI descriptionText;
    public Button buyButton;

    private ShopUI shopUI;
    private ShopItemData itemData;

    public void Setup(ShopUI ui, ShopItemData item)
    {
        shopUI = ui;
        itemData = item;

        nameText.text = item.itemName;
        priceText.text = item.price + " Coin";
        descriptionText.text = item.description;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => shopUI.BuyItem(itemData));
    }
}