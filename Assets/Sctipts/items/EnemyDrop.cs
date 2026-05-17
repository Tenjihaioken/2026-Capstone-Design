using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [Header("드롭 테이블")]
    public DropItem[] dropItems;

    public void Drop()
    {
        if (dropItems == null || dropItems.Length == 0)
            return;

        // 전체 확률 합 계산
        float totalChance = 0f;

        foreach (DropItem item in dropItems)
        {
            totalChance += item.dropChance;
        }

        // 랜덤 값 생성
        float randomValue = Random.Range(0f, totalChance);

        float currentChance = 0f;

        foreach (DropItem item in dropItems)
        {
            currentChance += item.dropChance;

            if (randomValue <= currentChance)
            {
                if (item.itemPrefab != null)
                {
                    Vector2 randomOffset = Random.insideUnitCircle * 0.5f;

                    Instantiate(
                        item.itemPrefab,
                        (Vector2)transform.position + randomOffset,
                        Quaternion.identity
                    );

                    Debug.Log($"{item.itemPrefab.name} 드롭!");
                }

                // 하나 드롭했으면 종료
                return;
            }
        }
    }
}