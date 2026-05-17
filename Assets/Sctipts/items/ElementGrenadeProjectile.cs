using System.Collections;
using UnityEngine;

public class ElementGrenadeProjectile : MonoBehaviour
{
    [Header("도착 판정")]
    public float arriveDistance = 0.1f;

    private Vector2 targetPosition;
    private float moveSpeed;
    private float explodeDelay;
    private GameObject areaPrefab;

    private bool initialized = false;
    private bool arrived = false;
    private bool exploded = false;

    public void Initialize(
        Vector2 startPosition,
        Vector2 target,
        float speed,
        float delay,
        GameObject area
    )
    {
        transform.position = startPosition;

        targetPosition = target;
        moveSpeed = speed;
        explodeDelay = delay;
        areaPrefab = area;

        initialized = true;
    }

    private void Update()
    {
        if (!initialized || arrived || exploded)
            return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPosition) <= arriveDistance)
        {
            Arrive();
        }
    }

    private void Arrive()
    {
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

    Debug.Log("속성 수류탄 폭발");

    if (areaPrefab != null)
    {
        Instantiate(areaPrefab, transform.position, Quaternion.identity);
        Debug.Log("장판 생성: " + areaPrefab.name);
    }
    else
    {
        Debug.LogWarning("Area Prefab이 비어 있습니다.");
    }

    Destroy(gameObject);
}
}