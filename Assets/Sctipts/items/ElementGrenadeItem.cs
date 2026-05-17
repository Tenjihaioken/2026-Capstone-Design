using UnityEngine;

[CreateAssetMenu(fileName = "ElementGrenadeItem", menuName = "Items/Element Grenade")]
public class ElementGrenadeItem : BaseItem
{
    [Header("수류탄 프리팹")]
    public GameObject grenadeProjectilePrefab;

    [Header("장판 프리팹")]
    public GameObject areaPrefab;

    [Header("수류탄 색상")]
    public Color grenadeColor = Color.white;

    [Header("투척 설정")]
    public float throwSpeed = 8f;
    public float explodeDelay = 1.2f;

    public override bool Use(GameObject user)
    {
        if (grenadeProjectilePrefab == null || areaPrefab == null)
        {
            Debug.LogWarning("수류탄 프리팹 또는 장판 프리팹이 연결되지 않았습니다.");
            return false;
        }

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
            return false;

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        Vector2 targetPosition = mouseWorldPos;

        GameObject grenadeObj = Instantiate(
            grenadeProjectilePrefab,
            user.transform.position,
            Quaternion.identity
        );

        ElementGrenadeProjectile projectile =
            grenadeObj.GetComponent<ElementGrenadeProjectile>();

        if (projectile != null)
        {
            projectile.Initialize(
                user.transform.position,
                targetPosition,
                throwSpeed,
                explodeDelay,
                areaPrefab
            );

            SpriteRenderer sr = grenadeObj.GetComponent<SpriteRenderer>();

            if (sr != null)
            {
                sr.color = grenadeColor;
            }
        }

        Debug.Log($"{itemName} 사용!");
        return true;
    }
}