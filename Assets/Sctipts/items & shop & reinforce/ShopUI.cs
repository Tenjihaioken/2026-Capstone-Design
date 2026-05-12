using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public GameObject shopPanel;

    [Header("슬롯")]
    public ShopSlotUI[] slots;

    private PlayerCore currentPlayer;
    private ShopItemData[] currentItems;

    private void Awake()
    {
        CloseShop();
    }

    public void OpenShop(PlayerCore player, ShopItemData[] items)
    {
        Debug.Log("상점 UI 열기");

        currentPlayer = player;
        currentItems = items;

        if (shopPanel != null)
            shopPanel.SetActive(true);
        else
            Debug.LogWarning("ShopPanel이 연결되지 않았습니다.");

        for (int i = 0; i < slots.Length; i++)
        {
            if (items != null && i < items.Length)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].Setup(this, items[i]);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }

        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void BuyItem(ShopItemData item)
    {
        if (currentPlayer == null || item == null)
            return;

        PlayerStats stats = currentPlayer.GetComponent<PlayerStats>();

        if (!stats.SpendCoin(item.price))
        {
            Debug.Log("코인이 부족합니다.");
            return;
        }

        Debug.Log(item.itemName + " 구매 완료");
    }
}