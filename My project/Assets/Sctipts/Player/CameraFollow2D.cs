using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("따라갈 대상")]
    public Transform target;
    public PlayerAim playerAim;

    [Header("기본 위치 설정")]
    public Vector3 baseOffset = new Vector3(0f, 0f, -10f);

    [Header("카메라 이동")]
    public float smoothTime = 0.15f;
    public float lookAheadDistance = 2.5f;

    [Header("카메라 회전")]
    public bool useRotation = true;
    public float maxRotateAngle = 8f;
    public float rotateSmoothSpeed = 8f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null || playerAim == null)
            return;

        FollowTarget();
        RotateCamera();
    }

    private void FollowTarget()
    {
        Vector2 aimDir = playerAim.AimDirection;

        Vector3 lookAheadOffset = new Vector3(
            aimDir.x * lookAheadDistance,
            aimDir.y * lookAheadDistance,
            0f
        );

        Vector3 targetPosition = target.position + baseOffset + lookAheadOffset;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }

    private void RotateCamera()
    {
        if (!useRotation)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0f, 0f, 0f),
                Time.deltaTime * rotateSmoothSpeed
            );
            return;
        }

        Vector2 aimDir = playerAim.AimDirection;

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        // 회전량을 너무 크게 주지 않기 위해 -1~1 범위 느낌으로 축소
        float normalized = Mathf.Clamp(aimDir.x, -1f, 1f);
        float zRotation = -normalized * maxRotateAngle;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, zRotation);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotateSmoothSpeed
        );
    }
}