using UnityEngine;
using UnityEngine.Tilemaps;

public class CorridorGenerator : MonoBehaviour
{
    public Tilemap floorTilemap;
    public TileBase floorTile; // 주원 님이 만든 중세 벽돌 타일 
    public int corridorWidth = 2; // 복도 두께 (2~3 권장)

    // 두 지점 사이의 'ㄴ'자 복도를 그리는 함수
    public void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        // 1. 수평 복도 생성 (x축 이동)
        int xDir = start.x < end.x ? 1 : -1;
        for (int x = start.x; x != end.x + xDir; x += xDir)
        {
            for (int w = 0; w < corridorWidth; w++)
            {
                floorTilemap.SetTile(new Vector3Int(x, start.y + w, 0), floorTile);
            }
        }

        // 2. 수직 복도 생성 (y축 이동)
        int yDir = start.y < end.y ? 1 : -1;
        for (int y = start.y; y != end.y + yDir; y += yDir)
        {
            for (int w = 0; w < corridorWidth; w++)
            {
                floorTilemap.SetTile(new Vector3Int(end.x + w, y, 0), floorTile);
            }
        }
    }
}