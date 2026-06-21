using UnityEngine;

[System.Serializable]
public class DialogueResponse
{
    public string responseText;
    public DialogueNode nextNode;


    public ItemData requiredItem;
    public int requiredItemAmount = 1;

   
    public QuestData requiredCompletedQuest;

    
    public QuestData questToStart;
}

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    [TextArea(2, 5)]
    public string npcLine;
    public DialogueResponse[] responses;
}