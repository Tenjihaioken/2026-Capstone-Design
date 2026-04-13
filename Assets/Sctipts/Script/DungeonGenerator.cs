using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [Header("방 데이터")]
    public RoomData startRoom;      // 시작 지점 설계도
    public List<RoomData> roomPool; // 무작위로 섞을 일반 방들 리스트
    public RoomData bossRoom;       // 마지막 보스 방 설계도

    [Header("방 규모 설정")]
    public int totalRooms = 10;     // 생성할 총 방의 개수
    public Vector2Int roomSpacing = new Vector2Int(20, 20); // 방 사이의 간격

    // 이미 방이 배치된 좌표를 저장 (중복 방지용)
    private HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Vector2Int currentPos = Vector2Int.zero; // (0,0)에서 시작
        
        // 1. 시작 방 생성
        PlaceRoom(startRoom, currentPos);

        // 2. 일반 방들 배치
        for (int i = 0; i < totalRooms - 2; i++)
        {
            // 무작위 방향 선택 (상하좌우)
            Vector2Int nextPos = GetNextPosition(currentPos);

            // 겹치지 않는 위치를 찾을 때까지 반복 (최대 10회 시도)
            int attempts = 0;
            while (occupiedPositions.Contains(nextPos) && attempts < 10)
            {
                nextPos = GetNextPosition(currentPos);
                attempts++;
            }

            // 새로운 위치에 무작위 방 배치
            RoomData randomData = roomPool[Random.Range(0, roomPool.Count)];
            PlaceRoom(randomData, nextPos);
            currentPos = nextPos; // 현재 위치 업데이트
        }

        // 3. 마지막 경로 끝에 보스 방 배치
        PlaceRoom(bossRoom, GetNextPosition(currentPos));
    }

    void PlaceRoom(RoomData data, Vector2Int pos)
    {
        // 논리적 좌표를 월드 좌표(Unity Scene)로 변환
        Vector3 worldPos = new Vector3(pos.x * roomSpacing.x, pos.y * roomSpacing.y, 0);
        
        // 프리팹 생성
        Instantiate(data.roomPrefab, worldPos, Quaternion.identity, transform);
        
        // 좌표 저장
        occupiedPositions.Add(pos);
    }

    Vector2Int GetNextPosition(Vector2Int current)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        return current + directions[Random.Range(0, directions.Length)];
    }
}