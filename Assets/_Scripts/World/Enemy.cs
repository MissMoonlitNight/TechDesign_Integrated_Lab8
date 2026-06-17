using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyType = "Goblin";
    public float health = 30f;
    public ItemData[] dropItems; // что выпадает при смерти

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        Debug.Log($"[Enemy] {enemyType} получил {dmg} урона. Осталось HP: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Дроп предметов в инвентарь игрока
        Inventory playerInv = FindObjectOfType<Inventory>();
        if (playerInv != null && dropItems != null)
        {
            foreach (var item in dropItems)
            {
                if (item != null) playerInv.AddItem(item, 1);
            }
        }

        //  Уведомляем квест-менеджер
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnEnemyKilled(enemyType);
        }

        Destroy(gameObject);
    }
}