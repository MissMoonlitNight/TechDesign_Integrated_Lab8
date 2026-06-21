using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Движение")]
    public float baseSpeed = 5f;

    private CharacterController controller;
    private PlayerStats playerStats;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();

    }

    void Update()
    {
        if (PauseMenu.Instance != null && (PauseMenu.Instance.IsInMainMenu() || PauseMenu.Instance.IsPaused()))
        {
            return;
        }

        if (IsAnyUIOpen())
        {
            return;
        }

        float currentSpeed = baseSpeed;
        if (playerStats != null)
        {
            currentSpeed = playerStats.speed;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Простая гравитация
        controller.Move(Vector3.up * Physics.gravity.y * Time.deltaTime);
    }

    // Проверка, открыт ли какой-либо UI
    bool IsAnyUIOpen()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.dialoguePanel.activeSelf)
            return true;

        if (UIManager.Instance != null && UIManager.Instance.IsAnyUIOpen())
            return true;

        var questPanel = GameObject.Find("QuestPanel");
        if (questPanel != null && questPanel.activeSelf) return true;

        var talentPanel = GameObject.Find("TalentPanel");
        if (talentPanel != null && talentPanel.activeSelf) return true;

        var craftPanel = GameObject.Find("CraftingPanel");
        if (craftPanel != null && craftPanel.activeSelf) return true;

        return false;
    }
}