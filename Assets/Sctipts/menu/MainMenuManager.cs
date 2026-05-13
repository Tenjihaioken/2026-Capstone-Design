using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("씬 이름")]
    public string gameSceneName = "DungeonGenerator";

    [Header("환경설정 패널")]
    public GameObject settingsPanel;

    private void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void StartNewGame()
    {
        SaveManager.Instance?.DeleteSave();

        PlayerPrefs.SetInt("LoadGame", 0);
        PlayerPrefs.Save();

        LoadScene(gameSceneName);
    }

    public void ContinueGame()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.HasSaveData())
        {
            SaveData data = SaveManager.Instance.LoadData();

            PlayerPrefs.SetInt("LoadGame", 1);
            PlayerPrefs.Save();

            string sceneToLoad = string.IsNullOrEmpty(data.currentSceneName)
                ? gameSceneName
                : data.currentSceneName;

            LoadScene(sceneToLoad);
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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void LoadScene(string sceneName)
    {
        if (SceneFadeManager.Instance != null)
            SceneFadeManager.Instance.FadeOutAndLoadScene(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }
}