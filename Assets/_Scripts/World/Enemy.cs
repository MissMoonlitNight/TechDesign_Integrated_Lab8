using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Настройки врага")]
    public string enemyType = "Goblin";
    public float health = 30f;
    public int expReward = 25; // опыт за убийство

    public ItemData[] dropItems; 

    private LevelSystem levelSystem; 

    void Start()
    {
    
        levelSystem = FindObjectOfType<LevelSystem>();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Дроп предметов
        Inventory playerInv = FindObjectOfType<Inventory>();
        if (playerInv != null && dropItems != null)
        {
            foreach (var item in dropItems)
            {
                if (item != null) playerInv.AddItem(item, 1);
            }
        }

        //  Уведомления для квест-менеджера
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnEnemyKilled(enemyType);
        }

        // Начисление опыта
        if (levelSystem != null)
        {
            levelSystem.AddExperience(expReward);
            Debug.Log($"[Enemy] Выдано {expReward} опыта за {enemyType}");
        }

        Destroy(gameObject);
    }
}