using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestLogUI : MonoBehaviour
{
    public Transform questListContainer;
    public GameObject questEntryPrefab;

    void Start()
    {
        Debug.Log("[QuestLogUI] Start вызван");
        // НЕ вызываем SetActive(false) — панель управляется через UIManager
    }

    // Этот метод вызывается из UIManager при открытии панели
    public void RefreshUI()
    {
        Debug.Log("[QuestLogUI] RefreshUI вызван");

        if (questListContainer == null)
        {
            Debug.LogError("[QuestLogUI] questListContainer не назначен!");
            return;
        }

        if (questEntryPrefab == null)
        {
            Debug.LogError("[QuestLogUI] questEntryPrefab не назначен!");
            return;
        }

        // Очистка старых записей
        foreach (Transform child in questListContainer)
            Destroy(child.gameObject);

        if (QuestManager.Instance == null)
        {
            Debug.LogError("[QuestLogUI] QuestManager.Instance = null!");
            return;
        }

        Debug.Log($"[QuestLogUI] Активных квестов: {QuestManager.Instance.activeQuests.Count}");

        if (QuestManager.Instance.activeQuests.Count == 0)
        {
            Debug.Log("[QuestLogUI] Нет активных квестов");
            return;
        }

        // Отрисовка активных квестов
        foreach (var quest in QuestManager.Instance.activeQuests)
        {
            if (quest == null) continue;

            Debug.Log($"[QuestLogUI] Отрисовка квеста: {quest.questName}");

            GameObject entry = Instantiate(questEntryPrefab, questListContainer);

            // Находим тексты по имени
            Text title = entry.transform.Find("Title").GetComponent<Text>();
            title.text = quest.questName;

            Text progressText = entry.transform.Find("Progress").GetComponent<Text>();
            string progStr = "";

            for (int i = 0; i < quest.goals.Length; i++)
            {
                int cur = QuestManager.Instance.GetGoalProgress(quest, i);
                string goalName = "";

                if (quest.goals[i].type == QuestGoal.GoalType.CollectItem)
                    goalName = quest.goals[i].requiredItem != null ? quest.goals[i].requiredItem.itemName : "Item";
                else if (quest.goals[i].type == QuestGoal.GoalType.KillEnemy)
                    goalName = quest.goals[i].enemyTag;

                progStr += goalName + ": " + cur + "/" + quest.goals[i].requiredAmount + "\n";
            }

            progressText.text = progStr;
        }
    }
}