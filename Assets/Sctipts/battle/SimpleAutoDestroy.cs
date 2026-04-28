using UnityEngine;

public class SimpleAutoDestroy : MonoBehaviour
{
    public float lifeTime = 0.1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}