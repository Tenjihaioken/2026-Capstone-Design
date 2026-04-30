using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI 참조")]
    public HeartUIManager heartUIManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RefreshHealthUI()
    {
        if (heartUIManager != null)
        {
            heartUIManager.Refresh();
        }
    }
}