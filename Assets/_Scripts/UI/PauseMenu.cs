using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [Header("UI Панели")]
    public GameObject pausePanel;      // Панель паузы
    public GameObject mainMenuPanel;   // Главное меню (при старте игры)

    private bool isPaused = false;
    private bool isInMainMenu = true;

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

    void Start()
    {
        // Показываем главное меню при старте
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
            isInMainMenu = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void Update()
    {
        // В главном меню пауза не работает
        if (isInMainMenu) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isPaused) Resume();
            else Pause();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance != null && UIManager.Instance.IsAnyUIOpen())
            {
                return;
            }

            if (isPaused) Resume();
            else Pause();
        }
    }

    public void StartGame()
    {
        // Скрываем главное меню
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }

        isInMainMenu = false;
        isPaused = false;

        // Блокируем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("[PauseMenu] Игра началась!");
    }

    public void Pause()
    {
        if (pausePanel == null) return;

        pausePanel.SetActive(true);
        isPaused = true;

        // Разблокируем курсор для работы с меню
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f; 

        Debug.Log("[PauseMenu] Игра на паузе");
    }

    public void Resume()
    {
        if (pausePanel == null) return;

        pausePanel.SetActive(false);
        isPaused = false;

        // Блокируем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f; 

        Debug.Log("[PauseMenu] Игра возобновлена");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("[PauseMenu] Игра перезапущена");
    }

    public void ExitToMainMenu()
    {

        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }

        isInMainMenu = true;
        isPaused = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("[PauseMenu] Возврат в главное меню");
    }

    public void ExitGame()
    {
        Debug.Log("[PauseMenu] Выход из игры");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool IsInMainMenu()
    {
        return isInMainMenu;
    }
}