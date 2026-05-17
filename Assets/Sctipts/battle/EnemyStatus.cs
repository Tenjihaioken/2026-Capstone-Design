using System.Collections;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public bool IsFrozen { get; private set; } = false;
    public float SpeedMultiplier { get; private set; } = 1f;

    private Coroutine freezeCoroutine;
    private Coroutine slowCoroutine;

    public void ApplyFreeze(float duration)
    {
        if (freezeCoroutine != null)
            StopCoroutine(freezeCoroutine);

        freezeCoroutine = StartCoroutine(FreezeCoroutine(duration));
    }

    public void ApplySlow(float duration, float slowMultiplier)
    {
        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(SlowCoroutine(duration, slowMultiplier));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        IsFrozen = true;
        yield return new WaitForSeconds(duration);
        IsFrozen = false;
    }

    private IEnumerator SlowCoroutine(float duration, float slowMultiplier)
    {
        SpeedMultiplier = slowMultiplier;
        yield return new WaitForSeconds(duration);
        SpeedMultiplier = 1f;
    }
}