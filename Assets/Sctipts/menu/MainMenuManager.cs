using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("씬 이름")]
    public string gameSceneName = "PlayerDebugScene";

    [Header("환경설정 패널")]
    public GameObject settingsPanel;

    private void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteKey("HasSaveData");

        if (SceneFadeManager.Instance != null)
            SceneFadeManager.Instance.FadeOutAndLoadScene(gameSceneName);
        else
            SceneManager.LoadScene(gameSceneName);
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("HasSaveData"))
        {
            if (SceneFadeManager.Instance != null)
                SceneFadeManager.Instance.FadeOutAndLoadScene(gameSceneName);
            else
                SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다.");
        }
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("게임 종료");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}