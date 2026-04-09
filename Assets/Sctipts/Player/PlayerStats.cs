using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("기본 능력치")]
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;

    [Header("전투 능력치")]
    public int maxHp = 10;
    public int currentHp = 10;
    public int attackPower = 1;
    public float dashCooldown = 1f;
    public bool isInvincible = false;

    [Header("재화")]
    public int coin = 0;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if (currentHp > maxHp)
            currentHp = maxHp;
    }

    public void AddCoin(int amount)
    {
        coin += amount;
    }

    public bool SpendCoin(int amount)
    {
        if (coin < amount)
            return false;

        coin -= amount;
        return true;
    }

    private void Die()
    {
        Debug.Log("플레이어 사망");
        // 나중에 게임오버 처리 연결
    }
}