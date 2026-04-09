using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStats stats;
    private float lastDashTime;

    [Header("입력 키")]
    public KeyCode dashKey = KeyCode.Space;
    public KeyCode itemUseKey = KeyCode.Q;

    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;

    private bool isDashing = false;
    private bool canMove = true;

    public Vector2 LastMoveDirection => lastMoveDirection;
    public bool IsDashing => isDashing;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();

        // 게임 시작 직후 바로 대시 가능하게 하고 싶으면 이렇게 둬도 됨
        lastDashTime = -stats.dashCooldown;
    }

    private void Update()
    {
        HandleInput();

        if (Input.GetKeyDown(dashKey) && CanDash())
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(itemUseKey))
        {
            UseItem();
        }
    }

    private void FixedUpdate()
    {
        if (canMove && !isDashing)
        {
            rb.linearVelocity = moveInput * stats.moveSpeed;
        }
    }

    private void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(x, y).normalized;

        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }
    }

    private bool CanDash()
    {
        lastDashTime = -stats.dashCooldown;
        if (isDashing) return false;
        if (Time.time < lastDashTime + stats.dashCooldown) return false;

        return true;

    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canMove = false;
        lastDashTime = Time.time;
        stats.isInvincible = true;

        Vector2 dashDirection = moveInput != Vector2.zero ? moveInput : lastMoveDirection;
        rb.linearVelocity = dashDirection.normalized * stats.dashSpeed;

        yield return new WaitForSeconds(stats.dashDuration);

        rb.linearVelocity = Vector2.zero;
        stats.isInvincible = false;
        isDashing = false;
        canMove = true;
    }

    private void UseItem()
    {
        Debug.Log("아이템 사용 키 입력");
    }
}