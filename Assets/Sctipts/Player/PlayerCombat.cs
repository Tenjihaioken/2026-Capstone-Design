using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerCombat : MonoBehaviour
{
    private PlayerStats stats;
    private PlayerController controller;

    [Header("공격 설정")]
    public float attackRange = 1.2f;
    public float attackRadius = 0.5f;
    public float attackCooldown = 0.3f;
    public LayerMask enemyLayer;

    private float lastAttackTime;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;
        Attack();
    }

    private void Attack()
    {
        Vector2 attackDirection = controller.LastMoveDirection.normalized;
        Vector2 attackPoint = (Vector2)transform.position + attackDirection * attackRange;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint, attackRadius, enemyLayer);

        Debug.Log("공격 실행");

        foreach (Collider2D hit in hits)
        {
            EnemyDummy enemy = hit.GetComponent<EnemyDummy>();
            if (enemy != null)
            {
                enemy.TakeDamage(stats.attackPower);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        PlayerController pc = GetComponent<PlayerController>();
        if (pc == null) return;

        Gizmos.color = Color.red;
        Vector2 dir = pc.LastMoveDirection.normalized;
        Vector2 point = (Vector2)transform.position + dir * attackRange;
        Gizmos.DrawWireSphere(point, attackRadius);
    }
}