using UnityEngine;

[CreateAssetMenu(fileName = "PotionItem", menuName = "Items/Potion")]
public class PotionItem : BaseItem
{
    [Range(0.1f, 1f)]
    public float healRatio = 0.33f;

    public override bool Use(GameObject user)
    {
        PlayerStats stats = user.GetComponent<PlayerStats>();

        if (stats == null)
            return false;

        // 이미 풀피면 사용 안됨
        if (stats.currentHp >= stats.maxHp)
        {
            Debug.Log("이미 체력이 가득 차 있습니다.");
            return false;
        }

        int healAmount = Mathf.CeilToInt(stats.maxHp * healRatio);

        stats.Heal(healAmount);

        Debug.Log($"포션 사용! 체력 {healAmount} 회복");

        return true;
    }
}