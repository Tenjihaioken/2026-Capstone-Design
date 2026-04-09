using UnityEngine;

public class PlayerFlipToMouse : MonoBehaviour
{
    private Camera mainCamera;

    [Header("반전 대상")]
    public Transform visualTarget;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        FlipToMouse();
    }

    private void FlipToMouse()
    {
        if (visualTarget == null)
            return;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        if (mouseWorldPos.x >= visualTarget.position.x)
        {
            visualTarget.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            visualTarget.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}