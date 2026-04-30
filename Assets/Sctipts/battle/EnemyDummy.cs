using System.Collections;
using UnityEngine;

public class EnemyDummy : MonoBehaviour, IDamageable
{
    public int hp = 5;
    public int contactDamage = 1;

    [Header("피격 피드백")]
    public SpriteRenderer spriteRenderer;
    public Color hitColor = Color.red;
    public float hitFlashDuration = 0.08f;

    private Color originalColor;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"{gameObject.name} 이(가) {damage} 데미지를 받음. 남은 체력: {hp}");

        if (spriteRenderer != null)
        {
            StopAllCoroutines();
            StartCoroutine(HitFlash());
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitFlash()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} 사망");
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerCore player = collision.gameObject.GetComponent<PlayerCore>();
        if (player != null)
        {
            player.TakeDamage(contactDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerCore player = other.GetComponent<PlayerCore>();
        if (player != null)
        {
            player.TakeDamage(contactDamage);
        }
    }
}