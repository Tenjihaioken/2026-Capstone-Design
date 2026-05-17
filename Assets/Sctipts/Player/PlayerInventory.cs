using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("현재 아이템")]
    public BaseItem currentItem;

    [Header("현재 개수")]
    public int currentItemCount = 0;

    [Header("최대 보유")]
    public int maxItemCount = 5;

    public bool AddItem(BaseItem item, int amount = 1)
    {
        if (item == null)
            return false;

        if (currentItem == null)
        {
            currentItem = item;
            currentItemCount = Mathf.Min(amount, maxItemCount);

            Debug.Log($"{item.itemName} 획득!");
            return true;
        }

        if (currentItem.itemName == item.itemName)
        {
            if (currentItemCount >= maxItemCount)
            {
                Debug.Log("아이템을 더 이상 보유할 수 없습니다.");
                return false;
            }

            currentItemCount += amount;

            if (currentItemCount > maxItemCount)
                currentItemCount = maxItemCount;

            Debug.Log($"{item.itemName} 추가 획득! 현재 개수: {currentItemCount}");
            return true;
        }

        Debug.Log("이미 다른 아이템을 들고 있습니다.");
        return false;
    }

    public void UseCurrentItem(GameObject user)
    {
        if (currentItem == null)
        {
            Debug.Log("사용할 아이템이 없습니다.");
            return;
        }

        if (currentItemCount <= 0)
        {
            Debug.Log("아이템 개수가 없습니다.");
            return;
        }

        bool success = currentItem.Use(user);

        if (!success)
            return;

        currentItemCount--;

        Debug.Log($"{currentItem.itemName} 사용! 남은 개수: {currentItemCount}");

        if (currentItemCount <= 0)
        {
            currentItem = null;
            currentItemCount = 0;
        }
    }
}