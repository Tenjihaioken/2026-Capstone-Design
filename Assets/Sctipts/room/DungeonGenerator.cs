using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [Header("방 데이터")]
    public RoomData startRoom;
    public List<RoomData> roomPool;
    public RoomData bossRoom;

    [Header("규모 및 타일")]
    public int totalRooms = 15;
    public Vector2Int roomSpacing = new Vector2Int(30, 30);
    public Tilemap floorTilemap;
    public Tilemap wallTilemap;
    public TileBase floorTile;
    public TileBase wallTile;
    public int corridorWidth = 3;

    private HashSet<Vector2Int> occupiedPositions = new HashSet<Vector2Int>();
    // 방이 차지하는 모든 타일 좌표를 저장하여 벽 생성을 방지함
    private HashSet<Vector3Int> roomAreaTiles = new HashSet<Vector3Int>();

    void Start() => GenerateDungeon();

    void GenerateDungeon()
    {
        Vector2Int currentPos = Vector2Int.zero;
        RoomData currentData = startRoom;
        GameObject currentRoomObj = PlaceRoom(currentData, currentPos);

        for (int i = 0; i < totalRooms - 2; i++)
        {
            Vector2Int nextPos = GetNextPosition(currentPos);
            while (occupiedPositions.Contains(nextPos)) nextPos = GetNextPosition(currentPos);

            RoomData nextData = roomPool[Random.Range(0, roomPool.Count)];
            GameObject nextRoomObj = PlaceRoom(nextData, nextPos);
            
            // 정밀 연결 시작
            ConnectByDoors(currentRoomObj.transform.position, nextRoomObj.transform.position, currentData, nextData);

            currentPos = nextPos;
            currentData = nextData;
            currentRoomObj = nextRoomObj;
        }

        Vector2Int bossPos = GetNextPosition(currentPos);
        GameObject bossRoomObj = PlaceRoom(bossRoom, bossPos);
        ConnectByDoors(currentRoomObj.transform.position, bossRoomObj.transform.position, currentData, bossRoom);
    }

    void ConnectByDoors(Vector3 posA, Vector3 posB, RoomData dataA, RoomData dataB)
    {
        // 1. 실제 문 좌표 추출 (World To Cell)
        Vector3 doorAWorld = posA + (Vector3)(Vector2)dataA.doorPositions[0];
        Vector3 doorBWorld = posB + (Vector3)(Vector2)dataB.doorPositions[0];
        Vector3Int startCell = floorTilemap.WorldToCell(doorAWorld);
        Vector3Int endCell = floorTilemap.WorldToCell(doorBWorld);

        List<Vector3Int> path = new List<Vector3Int>();

        // 2. 문 밖으로 1칸 전진하여 입구 확보
        Vector3Int step = GetInitialStep(dataA.doorPositions[0], dataA.size);
        Vector3Int current = startCell + step;
        
        // 3. L자형 경로 생성 (X -> Y)
        DrawPath(startCell, current, path); // 문에서 1칸 나옴
        Vector3Int mid = new Vector3Int(endCell.x, current.y, 0);
        DrawPath(current, mid, path);
        DrawPath(mid, endCell, path);

        // 4. 벽 생성 (방 영역 제외)
        GenerateSafeWalls(path);
    }

    void DrawPath(Vector3Int from, Vector3Int to, List<Vector3Int> path)
    {
        int xStep = from.x < to.x ? 1 : (from.x > to.x ? -1 : 0);
        int yStep = from.y < to.y ? 1 : (from.y > to.y ? -1 : 0);

        int curX = from.x;
        int curY = from.y;

        while (curX != to.x || curY != to.y)
        {
            if (curX != to.x) curX += xStep;
            else if (curY != to.y) curY += yStep;

            for (int w = -corridorWidth / 2; w <= corridorWidth / 2; w++)
            {
                Vector3Int p = (curX != from.x) ? new Vector3Int(curX, curY + w, 0) : new Vector3Int(curX + w, curY, 0);
                floorTilemap.SetTile(p, floorTile);
                path.Add(p);
            }
        }
    }

    void GenerateSafeWalls(List<Vector3Int> path)
    {
        foreach (var p in path)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector3Int check = p + new Vector3Int(x, y, 0);
                    // 팩트: 방의 내부이거나 이미 바닥인 곳에는 벽을 세우지 않음
                    if (!roomAreaTiles.Contains(check) && floorTilemap.GetTile(check) == null)
                    {
                        wallTilemap.SetTile(check, wallTile);
                    }
                }
            }
        }
    }

    GameObject PlaceRoom(RoomData data, Vector2Int pos)
    {
        Vector3 worldPos = new Vector3(pos.x * roomSpacing.x, pos.y * roomSpacing.y, 0);
        GameObject room = Instantiate(data.roomPrefab, worldPos, Quaternion.identity, transform);
        occupiedPositions.Add(pos);

        // 방이 차지하는 영역을 타일 좌표로 기록하여 보호 구역 설정
        Vector3Int baseTile = floorTilemap.WorldToCell(worldPos);
        for (int x = 0; x < data.size.x; x++)
            for (int y = 0; y < data.size.y; y++)
                roomAreaTiles.Add(baseTile + new Vector3Int(x, y, 0));

        return room;
    }

    Vector3Int GetInitialStep(Vector2 door, Vector2Int size)
    {
        if (door.x <= 0) return Vector3Int.left;
        if (door.x >= size.x - 1) return Vector3Int.right;
        if (door.y <= 0) return Vector3Int.down;
        if (door.y >= size.y - 1) return Vector3Int.up;
        return Vector3Int.zero;
    }

    Vector2Int GetNextPosition(Vector2Int current)
    {
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        return current + dirs[Random.Range(0, dirs.Length)];
    }
}