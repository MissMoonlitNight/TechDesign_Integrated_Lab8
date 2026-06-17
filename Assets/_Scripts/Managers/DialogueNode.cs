using UnityEngine;

[System.Serializable]
public class DialogueResponse
{
    public string responseText;
    public DialogueNode nextNode;

    public QuestData questToStart; 
}

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    [TextArea(2, 5)]
    public string npcLine;
    public DialogueResponse[] responses;
}
