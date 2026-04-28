using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("이동")]
    public float moveSpeed = 5f;

    [Header("구르기")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("전투")]
    public int maxHp = 10;
    public int currentHp = 10;
    public int attackPower = 1;
    public float fireRate = 2f;
    public float projectileSpeed = 12f;

    [Header("피격")]
    public float damageInvincibleDuration = 0.5f;

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
    }
}