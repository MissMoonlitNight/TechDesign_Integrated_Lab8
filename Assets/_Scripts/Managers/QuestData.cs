using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/QuestData")]
public class QuestData : ScriptableObject
{
    public string questName;

    [TextArea(2, 4)]
    public string description;

    public QuestGoal[] goals;

    [Header("Награды")]
    public ItemData[] rewardItems;
    public int[] rewardAmounts;
    public int rewardExp = 50;         // опыт за выполнение
    public int rewardTalentPoints = 0; // очки талантов (для квеста на убийство)
}