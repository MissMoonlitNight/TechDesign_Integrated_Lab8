using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
    [Header("Настройки мыши")]
    public float mouseSensitivity = 2f;
    public Transform playerBody;

    [Header("Плавность")]
    public float smoothSpeed = 10f;

    private float xRotation = 0f;
    private float currentXRotation = 0f;
    private float currentYRotation = 0f;
    private float targetYRotation = 0f;
    private bool isCursorLocked = false;

    void Start()
    {
        isCursorLocked = false;
    }

    void Update()
    {

        if (PauseMenu.Instance != null && (PauseMenu.Instance.IsInMainMenu() || PauseMenu.Instance.IsPaused()))
        {
            // Разблокируем курсор, если он был заблокирован
            if (isCursorLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isCursorLocked = false;
            }
            return;
        }

        if (IsAnyUIOpen())
        {
            if (isCursorLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isCursorLocked = false;
            }
            return;
        }

        if (!isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isCursorLocked = true;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        targetYRotation += mouseX;

        currentXRotation = Mathf.Lerp(currentXRotation, xRotation, Time.deltaTime * smoothSpeed);
        currentYRotation = Mathf.Lerp(currentYRotation, targetYRotation, Time.deltaTime * smoothSpeed);

        transform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
        if (playerBody != null)
        {
            playerBody.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
        }
    }


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