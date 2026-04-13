using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("추적 설정")]
    public Transform player;      // 플레이어 오브젝트
    public float smoothTime = 0.15f; // 카메라가 따라오는 부드러움 정도 낮 빠름 높 느림)
    
    [Header("마우스 오프셋")]
    [Range(0f, 0.5f)]
    public float mouseWeight = 0.25f; // 마우스 방향으로 치우치는 정도 (0.2~0.3 권장)

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPos;

    void LateUpdate() // 캐릭터 이동이 끝난 후 실행되도록 LateUpdate 사용
    {
        if (player == null) return;

        // 플레이어 위치
        Vector3 playerPos = player.position;

        // 마우스의 월드 좌표 
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 2D므로 Z축 무시

        // 플레이어와 마우스 사이의 지점
        // Lerp를 사용하여 두 지점 사이의 특정 비율 지점을 계산함
        targetPos = Vector3.Lerp(playerPos, mousePos, mouseWeight);

        // 카메라의 Z값 유지 (-10 고정)
        targetPos.z = -10f;

        // SmoothDamp를 이용한 부드러운 이동
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}