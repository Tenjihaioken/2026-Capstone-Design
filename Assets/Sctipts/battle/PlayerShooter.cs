using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerStats stats;

    [Header("총알 프리팹")]
    public GameObject projectilePrefab;

    [Header("발사 이펙트")]
    public GameObject muzzleFlashPrefab;

    [Header("발사 위치")]
    public Transform firePoint;

    [Header("공격 설정")]
    public float fireRate = 2f;
    public float projectileSpeed = 12f;

    private float lastFireTime;

    private void Awake()
    {
        mainCamera = Camera.main;
        stats = GetComponent<PlayerStats>();
        lastFireTime = -999f;
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
    }

    private void TryShoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile Prefab이 연결되지 않았습니다.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("FirePoint가 연결되지 않았습니다.");
            return;
        }

        float fireDelay = 1f / fireRate;

        if (Time.time < lastFireTime + fireDelay)
            return;

        Shoot();
        lastFireTime = Time.time;
    }

    private void Shoot()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = (mouseWorldPos - firePoint.position).normalized;

        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(direction, stats.attackPower, projectileSpeed);
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectileObj.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        SpawnMuzzleFlash(angle);
    }

    private void SpawnMuzzleFlash(float angle)
    {
        if (muzzleFlashPrefab == null)
            return;

        GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, Quaternion.Euler(0f, 0f, angle));
    }
}