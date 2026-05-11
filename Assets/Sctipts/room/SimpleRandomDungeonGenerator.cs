using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SimpleRandomDungeonGenerator : MonoBehaviour
{
    [Header("맵 크기")]
    [SerializeField] public Vector2Int mapSize = new Vector2Int(50, 50);
    [SerializeField] private Vector2Int roomSizeMinMax = new Vector2Int(5, 15); // 최소, 최대 방 크기
    private int[,] mapData;

    [Header("맵 생성 옵션")]
    [SerializeField] private int maxRooms = 10; // 생성할 최대 방 개수
    [SerializeField, Range(1, 3)] private int shopRoomCount = 1;      // 상점 생성 개수
    [SerializeField, Range(1, 3)] private int specialRoomCount = 1;
    [SerializeField] private int corridorWidth = 2; // 복도 너비
    [SerializeField] private int roomSpacing = 3;

    [Header("특수 방 설정")]
    [SerializeField] private Vector2Int startRoomSize = new Vector2Int(10, 10);
    [SerializeField] private Vector2Int bossRoomSize = new Vector2Int(15, 15); // 보스방 전용 크기
    [SerializeField] private Vector2Int shopRoomSize = new Vector2Int(7, 7);   // 상점 크기 고정
    [SerializeField] private Vector2Int specialRoomSize = new Vector2Int(8, 8); // 특수방 크기 고정

    [Header("플레이어 및 특수 방 위치")]
    [SerializeField] private GameObject player;
    public RectInt startRoom;
    public RectInt bossRoom;

    [Header("맵 생성 타일 지정")]
    [SerializeField] private Tilemap tilemap; // 바닥
    [SerializeField] private Tilemap wallTilemap; // 벽 
    [SerializeField] private TileBase floorTile; // 바닥 타일 스프라이트 할당
    //[SerializeField] private TileBase wallTile;  // 벽 타일 스프라이트 할당
    [SerializeField] private RuleTile wallTile;

    [SerializeField] private List<RoomInstance> roomList = new List<RoomInstance>();
    private List<RectInt> createdRooms = new List<RectInt>();

    // 던전 타입을 명확히 구분하기 위한 상수
    private const int TYPE_WALL = 0;
    private const int TYPE_FLOOR = 1;

    private void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        if (tilemap == null || floorTile == null || wallTile == null)
        {
            Debug.LogError("Dungeon Generator: Please assign Tilemap, Floor Tile, and Wall Tile in the Inspector.");
            return;
        }

        tilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles(); 
    
        createdRooms.Clear();
        roomList.Clear(); // 리스트 초기화
    
        mapData = new int[mapSize.x, mapSize.y];

        // 1. 방 배치
        PlaceRandomRooms();

        // 방이 최소 2개 이상 생성되었을 때 역할 부여
    if (createdRooms.Count >= 2)
    {
        AssignRoomRoles();
        LocatePlayer();
        // 2. 복도 연결
        ConnectRooms();
        FindAllRoomDoors();
    }
        // 3. 타일 렌더링
        RenderDungeon();

    var combatManager = FindAnyObjectByType<RoomCombatManager>();
    if (combatManager != null) {
        combatManager.Initialize(roomList, tilemap, wallTilemap, floorTile, wallTile);
    } else {
    Debug.LogError("RoomCombatManager를 신(Scene)에서 찾을 수 없습니다!");
    }
    }


private void LocatePlayer()
{
    if (player == null) return;
    Vector2Int center = GetCenter(startRoom);
    // 타일맵 좌표를 월드 좌표로 변환 (mapSize/2 보정 포함)
    Vector3 worldPos = new Vector3(center.x - mapSize.x / 2f, center.y - mapSize.y / 2f, 0);
    player.transform.position = worldPos;
}

private void PlaceRandomRooms()
    {
        roomList.Clear();
    createdRooms.Clear();

    // 시작 방과 보스 방이 서로 마주 보게 할 구역 인덱스 (0: 좌상, 1: 우상, 2: 좌하, 3: 우하)
    int startCorner = Random.Range(0, 4);
    int bossCorner = 3 - startCorner; // 0<->3, 1<->2 대칭 구조 생성

    for (int i = 0; i < maxRooms; i++)
    {
        int w, h;
        RoomType type = RoomType.Combat;
        string name = "";

        // 방 타입 및 크기 결정 로직
        if (i == 0) { w = startRoomSize.x; h = startRoomSize.y; type = RoomType.Start; name = "Start Room"; }
        else if (i == 1) { w = bossRoomSize.x; h = bossRoomSize.y; type = RoomType.Boss; name = "Boss Room"; }
        else if (i <= 1 + shopRoomCount) { w = shopRoomSize.x; h = shopRoomSize.y; type = RoomType.Shop; name = $"Shop Room {i - 1}"; }
        else if (i <= 1 + shopRoomCount + specialRoomCount) { w = specialRoomSize.x; h = specialRoomSize.y; type = RoomType.Special; name = $"Special Room {i - (1 + shopRoomCount)}"; }
        else { w = Random.Range(roomSizeMinMax.x, roomSizeMinMax.y + 1); h = Random.Range(roomSizeMinMax.x, roomSizeMinMax.y + 1); name = $"Combat Room {i}"; }

        bool placed = false;
        int attempts = 0;

        while (!placed && attempts < 50)
        {
            int x, y;
            int margin = 2;

            // 시작 방(0)과 보스 방(1)만 구역을 제한하여 배치
            if (i == 0 || i == 1)
            {
                int targetCorner = (i == 0) ? startCorner : bossCorner;
                
                switch (targetCorner)
                {
                    case 0: // 좌상단
                        x = Random.Range(margin, mapSize.x / 4);
                        y = Random.Range(mapSize.y * 3 / 4, mapSize.y - h - margin);
                        break;
                    case 1: // 우상단
                        x = Random.Range(mapSize.x * 3 / 4, mapSize.x - w - margin);
                        y = Random.Range(mapSize.y * 3 / 4, mapSize.y - h - margin);
                        break;
                    case 2: // 좌하단
                        x = Random.Range(margin, mapSize.x / 4);
                        y = Random.Range(margin, mapSize.y / 4);
                        break;
                    default: // 우하단
                        x = Random.Range(mapSize.x * 3 / 4, mapSize.x - w - margin);
                        y = Random.Range(margin, mapSize.y / 4);
                        break;
                }
            }
            else // 일반 방들은 맵 전체에서 랜덤 배치
            {
                x = Random.Range(1, mapSize.x - w - 1);
                y = Random.Range(1, mapSize.y - h - 1);
            }

            RectInt newRoom = new RectInt(x, y, w, h);
            bool overlaps = false;
            foreach (var r in createdRooms)
            {
                // 간격(roomSpacing)을 고려한 겹침 체크
                if (newRoom.Overlaps(new RectInt(r.x - roomSpacing, r.y - roomSpacing, 
                    r.width + roomSpacing * 2, r.height + roomSpacing * 2)))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                createdRooms.Add(newRoom);
                roomList.Add(new RoomInstance(newRoom, type, name));
                
                // 맵 데이터에 바닥 타일 기록
                for (int tx = newRoom.x; tx < newRoom.xMax; tx++)
                    for (int ty = newRoom.y; ty < newRoom.yMax; ty++)
                        mapData[tx, ty] = TYPE_FLOOR;

                placed = true;
            }
            attempts++;
        }
        }
    }

private void ConnectRooms()
    {
        if (createdRooms.Count < 2) return;
        List<RectInt> unconnected = new List<RectInt>(createdRooms);
        List<RectInt> connected = new List<RectInt>();

        connected.Add(unconnected[0]);
        unconnected.RemoveAt(0);

        while (unconnected.Count > 0)
        {
            float minDistance = float.MaxValue;
            RectInt closestU = unconnected[0], closestC = connected[0];

            foreach (var c in connected)
            {
                foreach (var u in unconnected)
                {
                    float d = Vector2.Distance(GetCenter(c), GetCenter(u));
                    if (d < minDistance) { minDistance = d; closestU = u; closestC = c; }
                }
            }

            Vector2Int start = GetCenter(closestC);
            Vector2Int end = GetCenter(closestU);

            if (Random.value < 0.5f) {
                CreateHorizontalCorridor(start.x, end.x, start.y);
                CreateVerticalCorridor(start.y, end.y, end.x);
            } else {
                CreateVerticalCorridor(start.y, end.y, start.x);
                CreateHorizontalCorridor(start.x, end.x, end.y);
            }

            unconnected.Remove(closestU);
            connected.Add(closestU);
        }
    }

    private void CreateHorizontalCorridor(int x1, int x2, int y)
    {
        int startX = Mathf.Min(x1, x2);
        int endX = Mathf.Max(x1, x2);
        int halfWidth = corridorWidth / 2;

        for (int x = startX; x <= endX; x++)
        {
            for (int wy = -halfWidth; wy < halfWidth + (corridorWidth % 2); wy++)
            {
                if (IsWithinBounds(x, y + wy)) mapData[x, y + wy] = TYPE_FLOOR;
            }
        }
    }

    private void CreateVerticalCorridor(int y1, int y2, int x)
    {
        int startY = Mathf.Min(y1, y2);
        int endY = Mathf.Max(y1, y2);
        int halfWidth = corridorWidth / 2;

        for (int y = startY; y <= endY; y++)
        {
            for (int wx = -halfWidth; wx < halfWidth + (corridorWidth % 2); wx++)
            {
                if (IsWithinBounds(x + wx, y)) mapData[x + wx, y] = TYPE_FLOOR;
            }
        }
    }

private void RenderDungeon()
{
    tilemap.ClearAllTiles(); // 바닥 타일맵 
    wallTilemap.ClearAllTiles(); // 벽 전용 타일맵

    for (int x = 0; x < mapSize.x; x++)
    {
        for (int y = 0; y < mapSize.y; y++)
        {
            Vector3Int tilePos = new Vector3Int(x - mapSize.x / 2, y - mapSize.y / 2, 0);

            if (mapData[x, y] == TYPE_FLOOR)
            {
                tilemap.SetTile(tilePos, floorTile); // 길(바닥) 생성
            }
            else
            {
                // 주변 8칸 중 '바닥'이 하나라도 있다면 벽
                if (IsAdjacentToFloor(x, y))
                {
                    wallTilemap.SetTile(tilePos, wallTile); // 경계선에만 벽 생성
                }
            }
        }
    }
}

// 주변 8방향에 바닥이 있는지 체크
private bool IsAdjacentToFloor(int x, int y)
{
    for (int i = x - 1; i <= x + 1; i++)
    {
        for (int j = y - 1; j <= y + 1; j++)
        {
            // 맵 범위를 벗어나지 않는지 체크
            if (i >= 0 && i < mapSize.x && j >= 0 && j < mapSize.y)
            {
                if (mapData[i, j] == TYPE_FLOOR) return true;
            }
        }
    }
    return false;
}

private void AssignRoomRoles()
{
    if (createdRooms.Count < 2) return;
    // 1. 시작 및 보스 방 지정

    startRoom = createdRooms[0];
    roomList[0].type = RoomType.Start;
    roomList[0].roomName = "Start Room";

    // 2. 시작 방 좌표 동기화
    startRoom = roomList[0].rect;

    int bossIdx = -1;
    for (int i = 0; i < roomList.Count; i++)
    {
        if (roomList[i].type == RoomType.Boss)
        {
            bossIdx = i;
            break;
        }
    }

    // 3. 보스 방 확정
    var bossData = roomList.Find(r => r.type == RoomType.Boss);
    if (bossData != null)
    {
        bossRoom = bossData.rect;
        bossData.roomName = "Final Boss Room";
    }

    List<int> candidateIndices = new List<int>();
    for (int i = 1; i < roomList.Count; i++)
    {
        if (i != bossIdx) candidateIndices.Add(i);
    }

    Debug.Log($"시작 방 위치: {GetCenter(startRoom)}, 보스 방 위치: {GetCenter(bossRoom)}");
}


private void FindAllRoomDoors()
{
    foreach (var room in roomList)
    {
        room.doorPositions.Clear();
        RectInt r = room.rect;

        // 방의 4면 외곽 1칸씩 전수 조사
        // 상하 벽 조사
        for (int x = r.x; x < r.xMax; x++) {
            CheckAndAddDoor(x, r.y - 1, room); // 아래쪽
            CheckAndAddDoor(x, r.yMax, room); // 위쪽
        }
        // 좌우 벽 조사
        for (int y = r.y; y < r.yMax; y++) {
            CheckAndAddDoor(r.x - 1, y, room); // 왼쪽
            CheckAndAddDoor(r.xMax, y, room); // 오른쪽
        }
    }
}

private void CheckAndAddDoor(int x, int y, RoomInstance room)
{
    // 맵 범위 안이고, 그 좌표가 바닥(Floor)이라면 문이다.
    if (IsWithinBounds(x, y) && mapData[x, y] == TYPE_FLOOR)
    {
        room.doorPositions.Add(new Vector2Int(x, y));
    }
}

    private Vector2Int GetCenter(RectInt rect)
    {
        return new Vector2Int(rect.x + rect.width / 2, rect.y + rect.height / 2);
    }

    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < mapSize.x && y >= 0 && y < mapSize.y;
    }

private void OnDrawGizmos()
{
    if (roomList == null) return;

    foreach (var room in roomList)
    {
        // 타입별 색상 지정
        switch (room.type) {
            case RoomType.Start: Gizmos.color = Color.green; break;
            case RoomType.Boss: Gizmos.color = Color.red; break;
            case RoomType.Shop: Gizmos.color = Color.yellow; break;
            case RoomType.Special: Gizmos.color = Color.cyan; break;
            default: Gizmos.color = Color.white; break;
        }

        Vector3 center = new Vector3(room.rect.center.x - mapSize.x / 2f, room.rect.center.y - mapSize.y / 2f, 0);
        Vector3 size = new Vector3(room.rect.width, room.rect.height, 1);
        Gizmos.DrawWireCube(center, size);
        
        
    }
}

    // 에디터에서 테스트 버튼 (Optional)
    #if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(SimpleRandomDungeonGenerator))]
    public class DungeonGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SimpleRandomDungeonGenerator generator = (SimpleRandomDungeonGenerator)target;
            if (GUILayout.Button("Generate Dungeon"))
            {
                generator.GenerateDungeon();
            }
        }
    }
    #endif
}


public enum RoomType { Start, Combat, Boss, Shop, Special } // 방 종류 추가

[System.Serializable]
public class RoomInstance {
    public string roomName; // 인스펙터 식별 (예,) "Shop Room", "Combat Room 1")
    public RectInt rect;
    public RoomType type;
    public RoomState state;
    public bool isStartRoom = false;
    [HideInInspector] public List<Vector2Int> doorPositions = new List<Vector2Int>();
    
    public RoomInstance(RectInt rect, RoomType type, string name) {
        this.rect = rect;
        this.type = type;
        this.roomName = name;
    }
}

public enum RoomState { Empty, Combat, Cleared }