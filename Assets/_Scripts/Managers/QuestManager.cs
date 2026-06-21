using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<QuestData> availableQuests;
    public List<QuestData> activeQuests = new List<QuestData>();
    public List<QuestData> completedQuests = new List<QuestData>();

    private Dictionary<QuestData, int[]> goalProgress = new Dictionary<QuestData, int[]>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Подписываемся на события инвентаря
        Inventory inv = FindObjectOfType<Inventory>();
        if (inv != null)
        {
            inv.OnItemAdded += OnItemCollected;
            Debug.Log("[QuestManager] Подписался на OnItemAdded");
        }
        else
        {
            Debug.LogError("[QuestManager] Inventory не найден на сцене!");
        }
    }

    public void StartQuest(QuestData quest)
    {
        if (quest == null || activeQuests.Contains(quest) || completedQuests.Contains(quest)) return;

        activeQuests.Add(quest);
        int[] progress = new int[quest.goals.Length];
        goalProgress[quest] = progress;

        Debug.Log($"[QuestManager] Квест начат: {quest.questName}");

        // УБРАЛИ: CheckExistingProgress — стартовые ресурсы не засчитываются
    }

    // Событие от инвентаря
    public void OnItemCollected(ItemData item, int amount)
    {
        Debug.Log($"[QuestManager] Получено событие: {item.itemName} x{amount}");

        foreach (var quest in activeQuests.ToList())
        {
            bool updated = false;
            for (int i = 0; i < quest.goals.Length; i++)
            {
                var goal = quest.goals[i];
                if (goal.type == QuestGoal.GoalType.CollectItem && goal.requiredItem == item)
                {
                    Debug.Log($"[QuestManager] Цель совпадает! Прогресс: {goalProgress[quest][i]}/{goal.requiredAmount}");
                    int[] prog = goalProgress[quest];
                    prog[i] = Mathf.Min(prog[i] + amount, goal.requiredAmount);
                    updated = true;
                }
            }
            if (updated) TryCompleteQuest(quest);
        }
    }

    // Событие от врага
    public void OnEnemyKilled(string enemyType)
    {
        foreach (var quest in activeQuests.ToList())
        {
            bool updated = false;
            for (int i = 0; i < quest.goals.Length; i++)
            {
                var goal = quest.goals[i];
                if (goal.type == QuestGoal.GoalType.KillEnemy && goal.enemyTag == enemyType)
                {
                    int[] prog = goalProgress[quest];
                    prog[i] = Mathf.Min(prog[i] + 1, goal.requiredAmount);
                    updated = true;
                }
            }
            if (updated) TryCompleteQuest(quest);
        }
    }

    private void TryCompleteQuest(QuestData quest)
    {
        int[] prog = goalProgress[quest];
        for (int i = 0; i < quest.goals.Length; i++)
        {
            if (prog[i] < quest.goals[i].requiredAmount) return;
        }
        CompleteQuest(quest);
    }

    private void CompleteQuest(QuestData quest)
    {
        activeQuests.Remove(quest);
        goalProgress.Remove(quest);
        completedQuests.Add(quest);

        // Выдача предметов
        Inventory inv = FindObjectOfType<Inventory>();
        if (inv != null && quest.rewardItems != null)
        {
            for (int i = 0; i < quest.rewardItems.Length; i++)
            {
                inv.AddItem(quest.rewardItems[i], quest.rewardAmounts[i]);
            }
        }

        // Выдача опыта
        LevelSystem lvl = FindObjectOfType<LevelSystem>();
        if (lvl != null && quest.rewardExp > 0)
        {
            lvl.AddExperience(quest.rewardExp);
        }

        // Выдача очков талантов
        if (TalentManager.Instance != null && quest.rewardTalentPoints > 0)
        {
            TalentManager.Instance.availablePoints += quest.rewardTalentPoints;
            Debug.Log($"[QuestManager] Получено {quest.rewardTalentPoints} очк. талантов");
        }

        Debug.Log($"[QuestManager] Квест выполнен: {quest.questName}");
    }

    public int GetGoalProgress(QuestData quest, int goalIndex)
    {
        if (goalProgress.ContainsKey(quest))
            return goalProgress[quest][goalIndex];
        return 0;
    }
}