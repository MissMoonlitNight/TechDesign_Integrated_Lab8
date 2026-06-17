using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public Text npcText;
    public Transform responseContainer;
    public GameObject responseButtonPrefab;

    private DialogueNode currentNode;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueNode startNode)
    {
        if (startNode == null) return;
        currentNode = startNode;
        dialoguePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ShowNode(currentNode);
    }

    private void ShowNode(DialogueNode node)
    {
        if (node == null) return;
        npcText.text = node.npcLine;

        foreach (Transform child in responseContainer)
            Destroy(child.gameObject);

        if (node.responses == null || node.responses.Length == 0)
        {
            CreateCloseButton();
        }
        else
        {
            foreach (var resp in node.responses)
            {
                CreateResponseButton(resp);
            }
        }
    }

    private void CreateResponseButton(DialogueResponse resp)
    {
        GameObject btnObj = Instantiate(responseButtonPrefab, responseContainer);
        btnObj.GetComponentInChildren<Text>().text = resp.responseText;
        DialogueNode next = resp.nextNode;
        QuestData quest = resp.questToStart; // запоминаем квест

        btnObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            //  если в ответе указан квест — начинаем его
            if (quest != null && QuestManager.Instance != null)
            {
                QuestManager.Instance.StartQuest(quest);
            }
            OnResponseSelected(next);
        });
    }

    private void CreateCloseButton()
    {
        GameObject closeBtn = Instantiate(responseButtonPrefab, responseContainer);
        closeBtn.GetComponentInChildren<Text>().text = "До свидания";
        closeBtn.GetComponent<Button>().onClick.AddListener(EndDialogue);
    }

    private void OnResponseSelected(DialogueNode nextNode)
    {
        if (nextNode == null) EndDialogue();
        else ShowNode(nextNode);
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentNode = null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}