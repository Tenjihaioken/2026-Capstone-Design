using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("패널")]
    public GameObject pausePanel;
    public GameObject settingsPanel;

    [Header("씬 이름")]
    public string mainMenuSceneName = "MainMenuScene";

    private bool isPaused = false;

    private void Start()
    {
        ResumeGame();

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel != null && settingsPanel.activeSelf)
            {
                CloseSettings();
                return;
            }

            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
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

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        if (SceneFadeManager.Instance != null)
            SceneFadeManager.Instance.FadeOutAndLoadScene(mainMenuSceneName);
        else
            SceneManager.LoadScene(mainMenuSceneName);
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
    public void SaveGame()
{
    PlayerCore player = FindFirstObjectByType<PlayerCore>();

    if (player != null)
    {
        SaveManager.Instance?.SaveGame(player);
    }
}
}