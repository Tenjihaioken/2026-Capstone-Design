using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TilemapRoomTrigger : MonoBehaviour
{
    public TilemapMonsterSpawner spawner;

    private bool triggered = false;

    private void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;

        if (spawner != null)
        {
            spawner.SpawnMonsters();
        }
        else
        {
            Debug.LogWarning("TilemapMonsterSpawner가 연결되지 않았습니다.");
        }
    }
}