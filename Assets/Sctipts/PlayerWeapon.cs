using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerWeapon : MonoBehaviour
{
    [Header("무기 정보")]
    public string weaponName = "기본 검";

    [SerializeField]
    private int upgradeLevel = 0;

    public int MaxUpgradeLevel => maxUpgradeLevel;
    public int UpgradeLevel => upgradeLevel;

    [Header("강화 설정")]
    public int maxUpgradeLevel = 5;
    public int attackIncreasePerUpgrade = 1;

    private PlayerStats stats;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();

        if (stats == null)
        {
            Debug.LogError("PlayerWeapon: PlayerStats 필요");
        }
    }

    public bool CanUpgrade()
    {
        return upgradeLevel < maxUpgradeLevel;
    }

    public void UpgradeWeapon()
    {
        if (!CanUpgrade()) return;

        upgradeLevel++;
        stats.attackPower += attackIncreasePerUpgrade;

        Debug.Log(
            $"강화 성공: +{upgradeLevel} | 공격력: {stats.attackPower}"
        );
    }
}