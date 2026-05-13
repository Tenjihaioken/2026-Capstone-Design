using UnityEngine;

public class GameSaveLoader : MonoBehaviour
{
    public PlayerCore playerCore;

    private void Start()
    {
        if (PlayerPrefs.GetInt("LoadGame", 0) != 1)
            return;

        PlayerPrefs.SetInt("LoadGame", 0);
        PlayerPrefs.Save();

        if (SaveManager.Instance == null)
            return;

        SaveData data = SaveManager.Instance.LoadData();

        if (data != null)
        {
            SaveManager.Instance.ApplySaveData(playerCore, data);
        }
    }
}