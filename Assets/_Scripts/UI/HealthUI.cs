using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("—сылки на UI")]
    public Text healthText;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats != null)
        {
            UpdateUI();
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
        if (healthText != null)
        {
            healthText.text = $"HP: {playerStats.currentHealth}/{playerStats.maxHealth}";
        }
    }
}