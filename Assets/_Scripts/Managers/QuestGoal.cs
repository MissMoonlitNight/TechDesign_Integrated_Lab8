using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public enum GoalType { CollectItem, KillEnemy, TalkToNPC }

    public GoalType type;
    public ItemData requiredItem;    // для CollectItem
    public string enemyTag;          // для KillEnemy и TalkToNPC
    public int requiredAmount;
}