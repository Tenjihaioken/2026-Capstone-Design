using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private PlayerCore playerCore;

    [Header("이동")]
    public float moveSpeed = 2f;
    public float stopDistance = 3f;

    [Header("공격")]
    public float attackCooldown = 1.5f;
    public int damage = 1;

    private float lastAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerCore = playerObj.GetComponent<PlayerCore>();
        }
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            TryAttack();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        if (playerCore != null)
        {
            playerCore.TakeDamage(damage);
        }

        lastAttackTime = Time.time;
    }
}