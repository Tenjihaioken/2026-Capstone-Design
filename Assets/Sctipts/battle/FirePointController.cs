using UnityEngine;

public class FirePointController : MonoBehaviour
{
    private Camera mainCamera;

    [Header("기준 대상")]
    public Transform playerTransform;
    public Transform firePoint;

    [Header("거리 설정")]
    public float distanceFromPlayer = 0.6f;

    [Header("회전 설정")]
    public bool rotateFirePoint = true;
    public float rotationOffset = 0f;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateFirePointPosition();
    }

    private void UpdateFirePointPosition()
    {
        if (playerTransform == null || firePoint == null)
            return;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        Vector2 direction = (mouseWorldPos - playerTransform.position).normalized;

        if (direction.sqrMagnitude < 0.001f)
            return;

        firePoint.position = (Vector2)playerTransform.position + direction * distanceFromPlayer;

        if (rotateFirePoint)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
        }
    }
}