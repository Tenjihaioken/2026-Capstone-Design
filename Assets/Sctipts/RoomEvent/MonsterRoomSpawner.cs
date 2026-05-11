using System.Collections.Generic;
using UnityEngine;

public class MonsterRoomSpawner : MonoBehaviour
{
    [Header("스폰할 몬스터 프리팹")]
    public GameObject[] monsterPrefabs;

    [Header("스폰 위치")]
    public Transform[] spawnPoints;

    [Header("스폰 설정")]
    public int maxMonsterCount = 5;
    public bool spawnOnlyOnce = true;

    private bool hasSpawned = false;
    private readonly List<GameObject> spawnedMonsters = new List<GameObject>();

    public void SpawnMonsters()
    {
        if (spawnOnlyOnce && hasSpawned)
            return;

        if (monsterPrefabs == null || monsterPrefabs.Length == 0)
        {
            Debug.LogWarning("스폰할 몬스터 프리팹이 없습니다.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("스폰 위치가 없습니다.");
            return;
        }

        hasSpawned = true;

        int spawnCount = Mathf.Min(maxMonsterCount, spawnPoints.Length);

        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for (int i = 0; i < spawnCount; i++)
        {
            int pointIndex = Random.Range(0, availablePoints.Count);
            Transform spawnPoint = availablePoints[pointIndex];
            availablePoints.RemoveAt(pointIndex);

            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

            GameObject monster = Instantiate(
                monsterPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            spawnedMonsters.Add(monster);
        }
    }

    public int GetAliveMonsterCount()
    {
        spawnedMonsters.RemoveAll(monster => monster == null);
        return spawnedMonsters.Count;
    }

    public void SpawnMonstersInRoom(RectInt roomRect, int mapSizeX, int mapSizeY)
    {
    if (spawnOnlyOnce && hasSpawned) return;
    spawnedMonsters.Clear();
    hasSpawned = false;

    // 방 내부의 유효한 타일 좌표들 계산 (벽에서 1칸씩 떨어짐)
    List<Vector2Int> validTiles = new List<Vector2Int>();
    for (int x = roomRect.x + 1; x < roomRect.xMax - 1; x++)
    {
        for (int y = roomRect.y + 1; y < roomRect.yMax - 1; y++)
        {
            validTiles.Add(new Vector2Int(x, y));
        }
    }

    int spawnCount = Mathf.Min(maxMonsterCount, validTiles.Count);

    for (int i = 0; i < spawnCount; i++)
    {
        if (validTiles.Count == 0) break;

        int randIdx = Random.Range(0, validTiles.Count);
        Vector2Int gridPos = validTiles[randIdx];
        validTiles.RemoveAt(randIdx);

        // 그리드 좌표를 월드 좌표로 변환
        Vector3 spawnPos = new Vector3(gridPos.x - mapSizeX / 2f + 0.5f, gridPos.y - mapSizeY / 2f + 0.5f, 0);

        GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
        GameObject monster = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedMonsters.Add(monster);
        }
    }

    public void ClearSpawnedList()
    {
    
    spawnedMonsters.Clear(); 
    
    Debug.Log("소환된 몬스터 리스트가 초기화되었습니다.");
    }
}