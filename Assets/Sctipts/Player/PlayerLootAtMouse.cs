using UnityEngine;

public class PlayerLookAtMouse : MonoBehaviour
{
    private Camera mainCamera;

    [Header("회전 설정")]
    public bool rotatePlayer = true;
    public float rotationOffset = -90f;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        LookAtMouse();
    }

    private void LookAtMouse()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        Vector2 direction = mouseWorldPos - transform.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (rotatePlayer)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
        }
    }
}