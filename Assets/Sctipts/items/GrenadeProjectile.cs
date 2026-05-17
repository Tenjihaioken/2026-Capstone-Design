using System.Collections;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [Header("Æø¹ß ´ë»ó")]
    public LayerMask enemyLayer;

    [Header("µµÂø ÆÇÁ¤")]
    public float arriveDistance = 0.1f;

    private Vector2 targetPosition;
    private float moveSpeed;
    private float explodeDelay;
    private float explosionRadius;
    private int damage;

    private bool initialized = false;
    private bool arrived = false;
    private bool exploded = false;

    public void Initialize(
        Vector2 startPosition,
        Vector2 target,
        float speed,
        float delay,
        float radius,
        int damageAmount
    )
    {
        transform.position = startPosition;

        targetPosition = target;
        moveSpeed = speed;
        explodeDelay = delay;
        explosionRadius = radius;
        damage = damageAmount;

        initialized = true;
    }

    private void Update()
    {
        if (!initialized || exploded || arrived)
            return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        float distance = Vector2.Distance(transform.position, targetPosition);

        if (distance <= arriveDistance)
        {
            Arrive();
        }
    }

    private void Arrive()
    {
        if (arrived)
            return;

        arrived = true;
        transform.position = targetPosition;

        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explodeDelay);
        Explode();
    }

    private void Explode()
    {
        if (exploded)
            return;

        exploded = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            explosionRadius,
            enemyLayer
        );

        Debug.Log($"¼ö·ùÅº Æø¹ß! ¸ÂÀº ´ë»ó ¼ö: {hits.Length}");

        foreach (Collider2D hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}