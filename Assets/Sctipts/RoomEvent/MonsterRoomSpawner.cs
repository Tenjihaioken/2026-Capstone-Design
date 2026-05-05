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
}