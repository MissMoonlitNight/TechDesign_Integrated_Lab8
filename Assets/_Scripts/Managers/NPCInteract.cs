using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public DialogueNode startNode;
    private bool playerInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (DialogueManager.Instance != null && startNode != null)
                DialogueManager.Instance.StartDialogue(startNode);
        }
    }
}