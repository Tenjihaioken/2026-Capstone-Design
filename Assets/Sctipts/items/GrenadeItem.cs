using UnityEngine;

[CreateAssetMenu(fileName = "GrenadeItem", menuName = "Items/Grenade")]
public class GrenadeItem : BaseItem
{
    [Header("수류탄 프리팹")]
    public GameObject grenadeProjectilePrefab;

    [Header("투척 설정")]
    public float throwSpeed = 8f;
    public float explodeDelay = 1.2f;

    [Header("폭발 설정")]
    public float explosionRadius = 2f;
    public int damage = 3;

    public override bool Use(GameObject user)
    {
        if (grenadeProjectilePrefab == null)
        {
            Debug.LogWarning("수류탄 프리팹이 연결되지 않았습니다.");
            return false;
        }

        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogWarning("Main Camera를 찾을 수 없습니다.");
            return false;
        }

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

        Vector2 targetPosition = mouseWorldPos;

        GameObject grenadeObj = Instantiate(
            grenadeProjectilePrefab,
            user.transform.position,
            Quaternion.identity
        );

        GrenadeProjectile grenade = grenadeObj.GetComponent<GrenadeProjectile>();

        if (grenade != null)
        {
            grenade.Initialize(
                user.transform.position,
                targetPosition,
                throwSpeed,
                explodeDelay,
                explosionRadius,
                damage
            );
        }

        Debug.Log($"{itemName} 사용!");
        return true;
    }
}