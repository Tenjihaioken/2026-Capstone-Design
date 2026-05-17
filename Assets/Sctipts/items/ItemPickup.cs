using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("아이템 데이터")]
    public BaseItem item;

    [Header("획득 개수")]
    public int amount = 1;

    [Header("줍기 딜레이")]
    public float pickupDelay = 0.25f;

    private float spawnTime;

    private void Awake()
    {
        spawnTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time < spawnTime + pickupDelay)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory == null)
        {
            Debug.LogWarning("PlayerInventory가 없습니다.");
            return;
        }

        bool pickedUp = inventory.AddItem(item, amount);

        if (pickedUp)
        {
            Destroy(gameObject);
        }
    }
}