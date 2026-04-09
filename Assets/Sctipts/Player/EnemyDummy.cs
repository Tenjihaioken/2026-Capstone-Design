using UnityEngine;

public class EnemyDummy : MonoBehaviour
{
    public int hp = 3;

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log(gameObject.name + " 이(가) " + damage + " 데미지를 받음. 남은 체력: " + hp);

        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " 사망");
        Destroy(gameObject);
    }
}