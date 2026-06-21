using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    [Header("—сылки на UI")]
    public Text manaText;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats != null)
        {
            UpdateUI();
        }
        else
        {
            Debug.LogError("[ManaUI] PlayerStats не найден на сцене!");
        }
    }

    void Update()
    {
        if (playerStats != null)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (manaText != null)
        {
            manaText.text = $"Mana: {Mathf.FloorToInt(playerStats.currentMana)}/{Mathf.FloorToInt(playerStats.maxMana)}";
        }
    }
}