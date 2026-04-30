using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("투사체 설정")]
    public float speed = 8f;
    public float lifeTime = 3f;
    public int damage = 1;

    private Vector2 moveDirection;

    public void Initialize(Vector2 direction, int projectileDamage, float projectileSpeed)
    {
        moveDirection = direction.normalized;
        damage = projectileDamage;
        speed = projectileSpeed;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable target = other.GetComponent<IDamageable>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}