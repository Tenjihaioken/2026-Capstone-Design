using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel;

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI costText;

    public int baseCost = 10;
    public int costIncrease = 5;
    public int attackIncrease = 1;
    public int maxUpgradeLevel = 5;

    private PlayerCore currentPlayer;
    private PlayerStats stats;
    private int upgradeLevel = 0;

    private void Start()
    {
        CloseUpgrade();
    }

    public void OpenUpgrade(PlayerCore player)
    {
        currentPlayer = player;
        stats = player.GetComponent<PlayerStats>();

        upgradePanel.SetActive(true);
        Refresh();

        Time.timeScale = 0f;
    }

    public void UpgradeAttack()
    {
        if (stats == null)
            return;

        if (upgradeLevel >= maxUpgradeLevel)
        {
            Debug.Log("이미 최대 강화입니다.");
            return;
        }

        int cost = GetCurrentCost();

        if (!stats.SpendCoin(cost))
        {
            Debug.Log("코인이 부족합니다.");
            return;
        }

        upgradeLevel++;
        stats.attackPower += attackIncrease;

        Refresh();

        Debug.Log("무기 강화 완료. 현재 공격력: " + stats.attackPower);
    }

    private int GetCurrentCost()
    {
        return baseCost + upgradeLevel * costIncrease;
    }

    private void Refresh()
    {
        if (attackText != null)
            attackText.text = "공격력: " + stats.attackPower;

        if (costText != null)
        {
            if (upgradeLevel >= maxUpgradeLevel)
                costText.text = "최대 강화";
            else
                costText.text = "강화 비용: " + GetCurrentCost() + " Coin";
        }
    }

    public void CloseUpgrade()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        Time.timeScale = 1f;
    }
}