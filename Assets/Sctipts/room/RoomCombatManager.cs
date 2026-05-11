using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class RoomCombatManager : MonoBehaviour
{
    [SerializeField] private TileBase gateTile;

    private List<RoomInstance> rooms;
    private Tilemap floorMap, wallMap;
    private TileBase floorTile;
    private RuleTile wallTile;
    
    private Vector2Int mapSize; // 좌표 변환
    private RoomInstance currentRoom;
    private GameObject player;

    public void Initialize(List<RoomInstance> data, Tilemap fMap, Tilemap wMap, TileBase fTile, RuleTile wTile)
    {
        rooms = data;
        floorMap = fMap;
        wallMap = wMap;
        floorTile = fTile;
        wallTile = wTile;
        
        // 초기화 시 플레이어와 맵 사이즈 정보 가져옴.
        player = GameObject.FindGameObjectWithTag("Player");
        var generator = FindAnyObjectByType<SimpleRandomDungeonGenerator>();
        // Generator의 mapSize가 private라면 public 속성으로 변경하거나 직접 입력해야 합니다.
        if (generator != null)
        {
        // 생성기 실제 맵 사이즈를 동기화.
        this.mapSize = generator.mapSize; 
    } 

        Debug.Log($"Combat Manager 준비 완료. 방: {rooms?.Count ?? 0}개");
    }

    void Update()
    {

    if (player == null) {
        Debug.LogError("플레이어를 찾을 수 없음 태그 확인.");
        player = GameObject.FindGameObjectWithTag("Player"); // 재시도
        return;
    }

    if (rooms == null || rooms.Count == 0) {
        Debug.LogWarning("방 데이터가 비어있습니다.");
        return;
    }

        Vector2Int pPos = WorldToGrid(player.transform.position);

        foreach (var room in rooms)
        {
            if (room.isStartRoom || room.state != RoomState.Empty) continue;
            if (room.type == RoomType.Combat || room.type == RoomType.Boss)
        {
            if (room.rect.Contains(pPos))
            {
                if (IsDeepInside(pPos, room.rect))
                {
                    StartCombat(room);
                }
            }
        }
        }

        if (currentRoom != null && currentRoom.state == RoomState.Combat)
        {
            if (Input.GetKeyDown(KeyCode.K)) 
            {
                EndCombat(currentRoom);
            }
        }
    }

    private void StartCombat(RoomInstance room)
{
    room.state = RoomState.Combat;
    currentRoom = room;

    foreach (var pos in room.doorPositions)
    {
        // 그리드 좌표를 타일맵 좌표로 변환하여 벽 타일 배치
        wallMap.SetTile(GridToTilemapPos(pos), gateTile);
    }
    Debug.Log("전투 시작, 문이 닫혔습니다.");
}

    private void EndCombat(RoomInstance room)
    {
        room.state = RoomState.Cleared;
        currentRoom = null;

        // 문 열기 : 벽 타일 제거
        foreach (var pos in room.doorPositions)
        {
            Vector3Int tilePos = GridToTilemapPos(pos);
            wallMap.SetTile(tilePos, null);
        }

        Debug.Log("전투 종료, 문이 열렸습니다.");
    }

    // --- 유틸리티---
    private Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x + mapSize.x / 2f),
            Mathf.FloorToInt(worldPos.y + mapSize.y / 2f)
        );
    }

    private Vector3Int GridToTilemapPos(Vector2Int gridPos)
    {
        return new Vector3Int(gridPos.x - mapSize.x / 2, gridPos.y - mapSize.y / 2, 0);
    }

    private bool IsDeepInside(Vector2Int pPos, RectInt rect)
    {
        // 모서리에서 1칸 더 들어왔는지 확인하여 문에 끼는 현상 방지
        return pPos.x > rect.x && pPos.x < rect.xMax - 1 &&
               pPos.y > rect.y && pPos.y < rect.yMax - 1;
    }


}

