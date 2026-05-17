using UnityEngine;

public class IceArea : MonoBehaviour
{
    [Header("장판 지속시간")]
    public float areaDuration = 8f;

    [Header("효과 범위")]
    public float effectRadius = 2f;
    public LayerMask enemyLayer;

    [Header("빙결")]
    public float freezeDuration = 3f;

    [Header("슬로우")]
    public float slowDuration = 5f;

    [Range(0f, 1f)]
    public float slowMultiplier = 0.7f;

    private void Start()
    {
        Debug.Log("IceArea 생성됨");

        ApplyIceEffect();

        Destroy(gameObject, areaDuration);
    }

    private void ApplyIceEffect()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            effectRadius,
            enemyLayer
        );

        Debug.Log("IceArea 감지 대상 수: " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            EnemyStatus status = hit.GetComponent<EnemyStatus>();

            if (status == null)
            {
                Debug.LogWarning(hit.name + " 에 EnemyStatus가 없습니다.");
                continue;
            }

            status.ApplyFreeze(freezeDuration);
            status.ApplySlow(freezeDuration + slowDuration, slowMultiplier);

            Debug.Log(hit.name + " 빙결/슬로우 적용");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, effectRadius);
    }
}