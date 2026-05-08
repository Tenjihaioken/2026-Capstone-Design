using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapMonsterSpawner : MonoBehaviour
{
    [Header("타일맵")]
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;

    [Header("스폰할 몬스터 프리팹")]
    public GameObject[] monsterPrefabs;

    [Header("스폰 설정")]
    public int maxMonsterCount = 5;
    public bool spawnOnlyOnce = true;

    [Header("플레이어 근처 스폰 방지")]
    public Transform player;
    public float minDistanceFromPlayer = 3f;

    private bool hasSpawned = false;
    private readonly List<GameObject> spawnedMonsters = new List<GameObject>();

    public void SpawnMonsters()
    {
        if (spawnOnlyOnce && hasSpawned)
            return;

        if (floorTilemap == null || wallTilemap == null)
        {
            Debug.LogWarning("Floor Tilemap 또는 Wall Tilemap이 연결되지 않았습니다.");
            return;
        }

        if (monsterPrefabs == null || monsterPrefabs.Length == 0)
        {
            Debug.LogWarning("몬스터 프리팹이 등록되지 않았습니다.");
            return;
        }

        List<Vector3> validPositions = GetValidSpawnPositions();

        if (validPositions.Count == 0)
        {
            Debug.LogWarning("스폰 가능한 바닥 타일이 없습니다.");
            return;
        }

        hasSpawned = true;

        int spawnCount = Mathf.Min(maxMonsterCount, validPositions.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            int posIndex = Random.Range(0, validPositions.Count);
            Vector3 spawnPosition = validPositions[posIndex];
            validPositions.RemoveAt(posIndex);

            GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

            GameObject monster = Instantiate(
                monsterPrefab,
                spawnPosition,
                Quaternion.identity
            );

            spawnedMonsters.Add(monster);
        }
    }

    private List<Vector3> GetValidSpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();

        BoundsInt bounds = floorTilemap.cellBounds;

        foreach (Vector3Int cellPos in bounds.allPositionsWithin)
        {
            bool hasFloor = floorTilemap.HasTile(cellPos);
            bool hasWall = wallTilemap.HasTile(cellPos);

            if (!hasFloor)
                continue;

            if (hasWall)
                continue;

            Vector3 worldPos = floorTilemap.GetCellCenterWorld(cellPos);

            if (player != null)
            {
                float distance = Vector2.Distance(player.position, worldPos);
                if (distance < minDistanceFromPlayer)
                    continue;
            }

            positions.Add(worldPos);
        }

        return positions;
    }

    public int GetAliveMonsterCount()
    {
        spawnedMonsters.RemoveAll(monster => monster == null);
        return spawnedMonsters.Count;
    }
}