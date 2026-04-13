using UnityEngine;
using System.Collections.Generic;

// 유니티 프로젝트 메뉴에서 우클릭으로 생성
[CreateAssetMenu(fileName = "NewRoomData", menuName = "Dungeon/Room Data")]
public class RoomData : ScriptableObject
{
    // 방 유형
    public enum RoomType { Start, Combat, Shop, Treasure, Boss }
    public RoomType type;

    [Header("방 설정")]
    // 방의 크기
    public Vector2Int size; // 예: (10, 10), (15, 20) 등

    [Header("연결 설정")]
    // 문이 생성될 위치
    public List<Vector2Int> doorPositions = new List<Vector2Int>();

    [Header("시각적 요소")]
    // 데이터 기반 실제 방 프리팹
    public GameObject roomPrefab;
}