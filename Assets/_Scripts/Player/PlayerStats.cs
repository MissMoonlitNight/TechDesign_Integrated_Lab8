using UnityEngine;
using UnityEngine.UI; 

public class PlayerStats : MonoBehaviour
{
    [Header("Base Values (База)")]
    public float baseHealth = 100f;
    public float baseMana = 100f;
    public float baseDamage = 10f;
    public float baseSpeed = 5f;

    [Header("Current Values (Текущие)")]
    public float currentHealth;
    public float currentMana;

    [Header("Calculated Values (Readonly - с учетом талантов)")]
    public float maxHealth;
    public float maxMana;
    public float damage;
    public float speed;

    [Header("UI (Опционально)")]
    public Text healthText;
    public Text manaText;

    private float healthMod = 0f;
    private float manaMod = 0f;
    private float damageMod = 0f;
    private float speedMod = 0f;

    void Start()
    {
        Recalculate();
        currentHealth = maxHealth;
        currentMana = maxMana;
        UpdateUI();
    }

    public void AddModifier(string stat, float value)
    {
        switch (stat)
        {
            case "Health": healthMod += value; break;
            case "Mana": manaMod += value; break;
            case "Damage": damageMod += value; break;
            case "Speed": speedMod += value; break;
            default: Debug.LogWarning("Unknown stat: " + stat); break;
        }
        Recalculate();
    }

    private void Recalculate()
    {
        maxHealth = baseHealth + healthMod;
        maxMana = baseMana + manaMod;
        damage = baseDamage + damageMod;
        speed = baseSpeed + speedMod;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        UpdateUI();
    }

    public bool SpendMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void RestoreMana(float amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        UpdateUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateUI();

        if (currentHealth <= 0)
        {
            Debug.Log("Игрок погиб!");
        }
    }

    private void UpdateUI()
    {
        if (healthText != null) healthText.text = $"HP: {currentHealth}/{maxHealth}";
        if (manaText != null) manaText.text = $"Mana: {currentMana}/{maxMana}";
    }
}
