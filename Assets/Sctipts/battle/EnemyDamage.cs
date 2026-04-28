using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerCore player = collision.gameObject.GetComponent<PlayerCore>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerCore player = other.GetComponent<PlayerCore>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}