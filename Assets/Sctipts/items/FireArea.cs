using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArea : MonoBehaviour
{
    [Header("화염 장판")]
    public float duration = 5f;

    [Header("도트 데미지")]
    public float damageInterval = 1f;
    public int damagePerTick = 1;

    private List<IDamageable> targets =
        new List<IDamageable>();

    private void Start()
    {
        StartCoroutine(DamageCoroutine());

        Destroy(gameObject, duration);
    }

    private IEnumerator DamageCoroutine()
    {
        while (true)
        {
            foreach (IDamageable target in targets.ToArray())
            {
                if (target != null)
                {
                    target.TakeDamage(damagePerTick);

                    Debug.Log("화염 장판 데미지!");
                }
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable =
            other.GetComponent<IDamageable>();

        if (damageable != null &&
            !targets.Contains(damageable))
        {
            targets.Add(damageable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IDamageable damageable =
            other.GetComponent<IDamageable>();

        if (damageable != null &&
            targets.Contains(damageable))
        {
            targets.Remove(damageable);
        }
    }
}