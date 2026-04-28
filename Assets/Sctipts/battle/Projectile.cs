using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("총알 설정")]
    public float speed = 12f;
    public float lifeTime = 2f;
    public int damage = 1;

    [Header("히트 이펙트")]
    public GameObject hitEffectPrefab;

    private Vector2 moveDirection;

    public void Initialize(Vector2 direction, int attackDamage, float projectileSpeed)
    {
        moveDirection = direction.normalized;
        damage = attackDamage;
        speed = projectileSpeed;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyDummy enemy = other.GetComponent<EnemyDummy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            SpawnHitEffect();
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Wall"))
        {
            SpawnHitEffect();
            Destroy(gameObject);
        }
    }

    private void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
    }
}