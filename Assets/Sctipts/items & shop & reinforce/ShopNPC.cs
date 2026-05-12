using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable
{
    [Header("상점 UI")]
    public ShopUI shopUI;

    [Header("판매 아이템")]
    public ShopItemData[] shopItems;

    public void Interact(PlayerCore player)
    {
        Debug.Log("상점 NPC 상호작용 실행");

        if (shopUI == null)
        {
            Debug.LogWarning("ShopUI가 연결되지 않았습니다.");
            return;
        }

        shopUI.OpenShop(player, shopItems);
    }

    public string GetInteractText()
    {
        return "상점 이용하기 [F]";
    }
}