using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerCore : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private PlayerStats stats;
    private Camera mainCamera;

    [Header("입력 키")]
    public KeyCode dashKey = KeyCode.Space;
    public KeyCode itemUseKey = KeyCode.Q;

    [Header("참조")]
    public Transform visualTarget;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public GameObject muzzleFlashPrefab;
    public SpriteRenderer spriteRenderer;

    [Header("조준/회전")]
    public float playerRotationOffset = 0f;
    public float firePointDistance = 0.6f;
    public bool rotateFirePoint = true;
    public float firePointRotationOffset = 0f;

    [Header("피격 피드백")]
    public Color hitColor = Color.red;
    public float flashInterval = 0.05f;

    private Vector2 moveInput;
    private Vector2 aimDirection = Vector2.right;
    private Vector2 lastMoveDirection = Vector2.down;

    private bool isDashing = false;
    private bool canMove = true;
    private bool isDamageInvincible = false;

    private float lastDashTime;
    private float lastFireTime;

    private Color originalColor;

    public Vector2 AimDirection => aimDirection;
    public bool IsDashing => isDashing;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        mainCamera = Camera.main;

        lastDashTime = -stats.dashCooldown;
        lastFireTime = -999f;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        if (visualTarget == null)
            visualTarget = transform;
    }

    private void Update()
    {
        HandleInput();
        UpdateAim();
        UpdateLookRotation();
        UpdateFirePoint();

        HandleDashInput();
        HandleShootInput();
        HandleItemInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(x, y).normalized;

        if (moveInput != Vector2.zero)
            lastMoveDirection = moveInput;
    }

    private void HandleMovement()
    {
        if (canMove && !isDashing)
        {
            rb.linearVelocity = moveInput * stats.moveSpeed;
        }
    }

    private void UpdateAim()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        aimDirection = ((Vector2)mouseWorldPos - (Vector2)transform.position).normalized;

        if (aimDirection.sqrMagnitude < 0.001f)
            aimDirection = Vector2.right;
    }

    private void UpdateLookRotation()
    {
        if (visualTarget == null)
            return;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        visualTarget.rotation = Quaternion.Euler(0f, 0f, angle + playerRotationOffset);
    }

    private void UpdateFirePoint()
    {
        if (firePoint == null)
            return;

        firePoint.position = (Vector2)transform.position + aimDirection * firePointDistance;

        if (rotateFirePoint)
        {
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(0f, 0f, angle + firePointRotationOffset);
        }
    }

    private void HandleDashInput()
    {
        if (Input.GetKeyDown(dashKey) && CanDash())
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private bool CanDash()
    {
        if (isDashing)
            return false;

        if (Time.time < lastDashTime + stats.dashCooldown)
            return false;

        return true;
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        canMove = false;
        lastDashTime = Time.time;

        Vector2 dashDirection = moveInput != Vector2.zero ? moveInput : lastMoveDirection;
        rb.linearVelocity = dashDirection.normalized * stats.dashSpeed;

        yield return new WaitForSeconds(stats.dashDuration);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        canMove = true;
    }

    private void HandleShootInput()
    {
        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        float fireDelay = 1f / stats.fireRate;

        if (Time.time < lastFireTime + fireDelay)
            return;

        Shoot();
        lastFireTime = Time.time;
    }

    private void Shoot()
    {
        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(aimDirection, stats.attackPower, stats.projectileSpeed);
        }

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        projectileObj.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (muzzleFlashPrefab != null)
        {
            Instantiate(muzzleFlashPrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle));
        }
    }

    private void HandleItemInput()
    {
        if (Input.GetKeyDown(itemUseKey))
        {
            Debug.Log("아이템 사용 키 입력");
        }
    }

    public void TakeDamage(int damage)
{
    if (isDamageInvincible || isDashing)
        return;

    stats.TakeDamage(damage);

    UIManager.Instance?.RefreshHealthUI();

    if (stats.currentHp <= 0)
        return;

    StartCoroutine(DamageInvincibleCoroutine());
}

    private IEnumerator DamageInvincibleCoroutine()
    {
        isDamageInvincible = true;

        float elapsed = 0f;

        while (elapsed < stats.damageInvincibleDuration)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = hitColor;
                yield return new WaitForSeconds(flashInterval);

                spriteRenderer.color = originalColor;
                yield return new WaitForSeconds(flashInterval);
            }
            else
            {
                yield return null;
            }

            elapsed += flashInterval * 2f;
        }

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        isDamageInvincible = false;
    }
}