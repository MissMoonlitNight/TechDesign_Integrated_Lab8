using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Панели UI")]
    public GameObject inventoryPanel;
    public GameObject craftingPanel;
    public GameObject questPanel;
    public GameObject talentPanel;
    public GameObject dialoguePanel;

    private GameObject currentOpen;
    private bool isAnyUIOpen = false;

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
    }

    void Update()
    {
        // Если игра на паузе или в главном меню — игнорируем горячие клавиши UI
        if (PauseMenu.Instance != null && (PauseMenu.Instance.IsPaused() || PauseMenu.Instance.IsInMainMenu()))
        {
            return;
        }

        // Горячие клавиши для открытия панелей
        if (Input.GetKeyDown(KeyCode.I)) TogglePanel(inventoryPanel);
        if (Input.GetKeyDown(KeyCode.C)) TogglePanel(craftingPanel);
        if (Input.GetKeyDown(KeyCode.J)) TogglePanel(questPanel);
        if (Input.GetKeyDown(KeyCode.K)) TogglePanel(talentPanel);

        // Обработка клавиши Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Если открыта какая-то панель — просто закрываем её
            if (isAnyUIOpen)
            {
                CloseCurrent();
            }
        }
    }

    public void TogglePanel(GameObject panel)
    {
        if (panel == null) return;

        // Если эта панель уже открыта — закрываем её
        if (currentOpen == panel)
        {
            CloseCurrent();
            return;
        }

        // Закрываем текущее окно (если есть)
        if (currentOpen != null)
        {
            currentOpen.SetActive(false);
        }

        // Открываем новое
        panel.SetActive(true);
        currentOpen = panel;
        isAnyUIOpen = true;

        // ВАЖНО: Вызываем RefreshUI для специфичных панелей
        QuestLogUI questUI = panel.GetComponent<QuestLogUI>();
        if (questUI != null)
        {
            questUI.RefreshUI();
        }

        InventoryUI invUI = panel.GetComponent<InventoryUI>();
        if (invUI != null)
        {
            invUI.RefreshUI();
        }

        // Разблокируем курсор для работы с UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log($"[UIManager] Открыто: {panel.name}");
    }

    public void CloseCurrent()
    {
        if (currentOpen != null)
        {
            currentOpen.SetActive(false);
            currentOpen = null;
            isAnyUIOpen = false;

            // Блокируем курсор для управления камерой
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("[UIManager] Окно закрыто");
        }
    }

    public bool IsAnyUIOpen()
    {
        return isAnyUIOpen;
    }
}