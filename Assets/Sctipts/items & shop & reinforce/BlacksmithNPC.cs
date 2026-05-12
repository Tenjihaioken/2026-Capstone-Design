using UnityEngine;

public class BlacksmithNPC : MonoBehaviour, IInteractable
{
    public void Interact(PlayerCore player)
    {
        UpgradeUI upgradeUI = FindObjectOfType<UpgradeUI>();

        if (upgradeUI != null)
        {
            upgradeUI.OpenUpgrade(player);
        }
    }

    public string GetInteractText()
    {
        return "무기 강화하기 [F]";
    }
}