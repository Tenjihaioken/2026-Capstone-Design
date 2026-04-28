using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartUIManager : MonoBehaviour
{
    public PlayerStats playerStats;

    [Header("하트 설정")]
    public GameObject heartPrefab;
    public Transform heartParent;

    private List<Image> hearts = new List<Image>();

    private void Start()
    {
        InitHearts();
    }

    private void Update()
    {
        UpdateHearts();
    }

    private void InitHearts()
    {
        for (int i = 0; i < playerStats.maxHp; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartParent);
            Image img = heart.GetComponent<Image>();
            hearts.Add(img);
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].enabled = (i < playerStats.currentHp);
        }
    }
}