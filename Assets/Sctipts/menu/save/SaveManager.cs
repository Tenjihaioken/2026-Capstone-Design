using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string SavePath => Path.Combine(Application.persistentDataPath, "saveData.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveGame(PlayerCore playerCore)
    {
        if (playerCore == null)
        {
            Debug.LogWarning("저장 실패: PlayerCore가 없습니다.");
            return;
        }

        PlayerStats stats = playerCore.GetComponent<PlayerStats>();

        if (stats == null)
        {
            Debug.LogWarning("저장 실패: PlayerStats가 없습니다.");
            return;
        }

        SaveData data = new SaveData();

        data.maxHp = stats.maxHp;
        data.currentHp = stats.currentHp;
        data.attackPower = stats.attackPower;
        data.coin = stats.coin;
        data.moveSpeed = stats.moveSpeed;

        Vector3 pos = playerCore.transform.position;
        data.playerX = pos.x;
        data.playerY = pos.y;
        data.playerZ = pos.z;

        data.currentSceneName = SceneManager.GetActiveScene().name;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);

        PlayerPrefs.SetInt("HasSaveData", 1);
        PlayerPrefs.Save();

        Debug.Log("게임 저장 완료: " + SavePath);
    }

    public bool HasSaveData()
    {
        return File.Exists(SavePath);
    }

    public SaveData LoadData()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("저장 파일이 없습니다.");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        return data;
    }

    public void ApplySaveData(PlayerCore playerCore, SaveData data)
    {
        if (playerCore == null || data == null)
            return;

        PlayerStats stats = playerCore.GetComponent<PlayerStats>();

        if (stats == null)
            return;

        stats.maxHp = data.maxHp;
        stats.currentHp = data.currentHp;
        stats.attackPower = data.attackPower;
        stats.coin = data.coin;
        stats.moveSpeed = data.moveSpeed;

        playerCore.transform.position = new Vector3(
            data.playerX,
            data.playerY,
            data.playerZ
        );

        UIManager.Instance?.RefreshHealthUI();

        Debug.Log("저장 데이터 적용 완료");
    }

    public void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }

        PlayerPrefs.DeleteKey("HasSaveData");
        PlayerPrefs.Save();

        Debug.Log("저장 데이터 삭제 완료");
    }
}