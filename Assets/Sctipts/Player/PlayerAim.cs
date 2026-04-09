using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Camera mainCamera;

    public Vector2 AimDirection { get; private set; } = Vector2.right;
    public Vector2 MouseWorldPosition { get; private set; }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateAim();
    }

    private void UpdateAim()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);

        MouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        Vector2 playerPos = transform.position;
        AimDirection = (MouseWorldPosition - playerPos).normalized;
    }
}