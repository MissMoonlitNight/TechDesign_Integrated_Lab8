using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public Text npcText;
    public Transform responseContainer;
    public GameObject responseButtonPrefab;

    private DialogueNode currentNode;
    private Inventory playerInventory;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        dialoguePanel.SetActive(false);
        playerInventory = FindObjectOfType<Inventory>();
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
            bool hasValidResponses = false;

            foreach (var resp in node.responses)
            {

                if (resp.requiredItem != null && (playerInventory == null || !playerInventory.HasItem(resp.requiredItem, resp.requiredItemAmount)))
                {
                    continue;
                }

                if (resp.requiredCompletedQuest != null)
                {
                    if (QuestManager.Instance == null ||
                        !QuestManager.Instance.completedQuests.Contains(resp.requiredCompletedQuest))
                    {
                        continue; 
                    }
                }

              
                if (resp.questToStart != null)
                {
                    if (QuestManager.Instance != null)
                    {
                        if (QuestManager.Instance.activeQuests.Contains(resp.questToStart) ||
                            QuestManager.Instance.completedQuests.Contains(resp.questToStart))
                        {
                            continue;
                        }
                    }
                }

                hasValidResponses = true;
                CreateResponseButton(resp);
            }

            if (!hasValidResponses)
            {
                CreateCloseButton();
            }
        }
    }

    private void CreateResponseButton(DialogueResponse resp)
    {
        GameObject btnObj = Instantiate(responseButtonPrefab, responseContainer);
        btnObj.GetComponentInChildren<Text>().text = resp.responseText;

        DialogueNode next = resp.nextNode;
        QuestData quest = resp.questToStart;

        btnObj.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (quest != null && QuestManager.Instance != null)
            {
                if (!QuestManager.Instance.activeQuests.Contains(quest) &&
                    !QuestManager.Instance.completedQuests.Contains(quest))
                {
                    QuestManager.Instance.StartQuest(quest);
                }
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
        if (nextNode == null)
        {
            EndDialogue();
        }
        else
        {
            currentNode = nextNode;
            ShowNode(currentNode);
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentNode = null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}