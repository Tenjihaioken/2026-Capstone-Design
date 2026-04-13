using UnityEngine;
using TMPro;

public class Blacksmith : MonoBehaviour
{
    [Header("공용 UI 텍스트")]
    public TextMeshProUGUI interactionText;

    [Header("머리 위 위치")]
    public Transform headPoint;
    public Vector3 textOffset = new Vector3(0f, 1.5f, 0f);

    [Header("입력 키")]
    public KeyCode interactKey = KeyCode.Y;

    [Header("강화 비용")]
    public int baseCost = 10;
    public int costPerLevel = 5;

    [Header("대사")]
    [TextArea] public string idleLine = "무기를 손보겠나? [Y]";
    [TextArea] public string noDataLine = "강화 정보를 찾을 수 없습니다.";
    [TextArea] public string maxUpgradeLine = "이미 최대 강화 단계입니다.";
    [TextArea] public string notEnoughCoinLine = "재화가 부족하군.";
    [TextArea] public string successLine = "좋아, 더 날카로워졌군.";

    private bool playerInRange = false;

    private PlayerStats stats;
    private PlayerWeapon weapon;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            UpdateTextPosition();

            if (Input.GetKeyDown(interactKey))
            {
                Interact();
            }
        }
    }

    private void Interact()
    {
        if (stats == null || weapon == null)
        {
            ShowText(noDataLine);
            return;
        }

        if (!weapon.CanUpgrade())
        {
            ShowText(maxUpgradeLine);
            return;
        }

        int cost = GetUpgradeCost();

        if (!stats.SpendCoin(cost))
        {
            ShowText($"{notEnoughCoinLine}\n필요 코인: {cost}");
            return;
        }

        weapon.UpgradeWeapon();

        ShowText(
            $"{successLine}\n" +
            $"{weapon.weaponName} +{weapon.UpgradeLevel}\n" +
            $"공격력: {stats.attackPower}\n" +
            $"남은 코인: {stats.coin}"
        );
    }

    private int GetUpgradeCost()
    {
        if (weapon == null)
            return baseCost;

        return baseCost + (weapon.UpgradeLevel * costPerLevel);
    }

    private void ShowText(string message)
    {
        if (interactionText == null)
            return;

        interactionText.gameObject.SetActive(true);
        interactionText.text = message;
        UpdateTextPosition();
    }

    private void HideText()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    private void UpdateTextPosition()
    {
        if (interactionText == null || !interactionText.gameObject.activeSelf)
            return;

        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 worldPos = headPoint != null ? headPoint.position : transform.position + textOffset;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        if (screenPos.z > 0f)
        {
            interactionText.transform.position = screenPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;
        stats = other.GetComponent<PlayerStats>();
        weapon = other.GetComponent<PlayerWeapon>();

        ShowText($"{idleLine}\n비용: {GetUpgradeCost()} 코인");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;
        stats = null;
        weapon = null;

        HideText();
    }

    private void OnDisable()
    {
        HideText();
    }
}