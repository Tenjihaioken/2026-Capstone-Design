using System.Collections;
using UnityEngine;

public enum RangedAttackPattern
{
    SingleShot,
    BurstShot,
    SpreadShot
}

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyRangedAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;

    [Header("이동 설정")]
    public float moveSpeed = 2f;
    public float attackDistance = 6f;
    public float retreatDistance = 3f;

    [Header("공격 설정")]
    public RangedAttackPattern attackPattern = RangedAttackPattern.SingleShot;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackCooldown = 2f;
    public float attackDelay = 0.6f;
    public int damage = 1;
    public float projectileSpeed = 8f;

    [Header("3점사 설정")]
    public int burstCount = 3;
    public float burstInterval = 0.15f;

    [Header("산탄 설정")]
    public int spreadCount = 5;
    public float spreadAngle = 45f;

    [Header("공격 경고 표시")]
    public GameObject warningLine;
    public float warningLength = 6f;
    public float warningWidth = 0.08f;

    [Header("회전 설정")]
    public bool rotateToPlayer = true;
    public float rotationOffset = 0f;

    private float lastAttackTime;
    private bool isPreparingAttack = false;
    private Vector2 lockedAttackDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        if (warningLine != null)
            warningLine.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (player == null || isPreparingAttack)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        HandleMovement();
    }

    private void Update()
    {
        if (player == null)
            return;

        LookAtPlayer();
        TryAttack();
    }

    private void HandleMovement()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        if (distance > attackDistance)
            rb.linearVelocity = directionToPlayer * moveSpeed;
        else if (distance < retreatDistance)
            rb.linearVelocity = -directionToPlayer * moveSpeed;
        else
            rb.linearVelocity = Vector2.zero;
    }

    private void TryAttack()
    {
        if (isPreparingAttack)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackDistance)
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        StartCoroutine(PrepareAndAttack());
    }

    private IEnumerator PrepareAndAttack()
    {
        isPreparingAttack = true;
        rb.linearVelocity = Vector2.zero;

        lockedAttackDirection = ((Vector2)player.position - (Vector2)firePoint.position).normalized;

        ShowWarningLine(lockedAttackDirection);

        yield return new WaitForSeconds(attackDelay);

        HideWarningLine();

        switch (attackPattern)
        {
            case RangedAttackPattern.SingleShot:
                Shoot(lockedAttackDirection);
                break;

            case RangedAttackPattern.BurstShot:
                yield return StartCoroutine(BurstShot(lockedAttackDirection));
                break;

            case RangedAttackPattern.SpreadShot:
                SpreadShot(lockedAttackDirection);
                break;
        }

        lastAttackTime = Time.time;
        isPreparingAttack = false;
    }

    private void Shoot(Vector2 direction)
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        EnemyProjectile projectile = projectileObj.GetComponent<EnemyProjectile>();
        if (projectile != null)
        {
            projectile.Initialize(direction, damage, projectileSpeed);
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectileObj.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private IEnumerator BurstShot(Vector2 direction)
    {
        for (int i = 0; i < burstCount; i++)
        {
            Shoot(direction);
            yield return new WaitForSeconds(burstInterval);
        }
    }

    private void SpreadShot(Vector2 centerDirection)
    {
        if (spreadCount <= 1)
        {
            Shoot(centerDirection);
            return;
        }

        float startAngle = -spreadAngle * 0.5f;
        float angleStep = spreadAngle / (spreadCount - 1);

        for (int i = 0; i < spreadCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 dir = RotateVector(centerDirection, angle);
            Shoot(dir);
        }
    }

    private Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;

        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        ).normalized;
    }

    private void ShowWarningLine(Vector2 direction)
    {
        if (warningLine == null)
            return;

        warningLine.SetActive(true);

        Vector3 centerPos = firePoint.position + (Vector3)(direction * warningLength * 0.5f);
        warningLine.transform.position = centerPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        warningLine.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        warningLine.transform.localScale = new Vector3(warningLength, warningWidth, 1f);
    }

    private void HideWarningLine()
    {
        if (warningLine != null)
            warningLine.SetActive(false);
    }

    private void LookAtPlayer()
    {
        if (!rotateToPlayer || isPreparingAttack)
            return;

        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
    }
}