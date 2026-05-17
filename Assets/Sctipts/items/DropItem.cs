using UnityEngine;

[System.Serializable]
public class DropItem
{
    [Header("드롭될 아이템 프리팹")]
    public GameObject itemPrefab;

    [Header("드롭 확률")]
    [Range(0f, 1f)]
    public float dropChance = 0.1f;
}