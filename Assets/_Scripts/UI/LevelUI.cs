using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [Header("—сылки на UI")]
    public Text levelText;
    public Text expText;

    private LevelSystem levelSystem;

    void Start()
    {
        levelSystem = FindObjectOfType<LevelSystem>();

        if (levelSystem != null)
        {
            levelSystem.OnLevelChanged += UpdateUI;
            UpdateUI(levelSystem.level, levelSystem.currentExp, levelSystem.expToNextLevel);
        }
    }

    void OnDestroy()
    {
        if (levelSystem != null)
            levelSystem.OnLevelChanged -= UpdateUI;
    }

    void UpdateUI(int lvl, int currentExp, int expToNext)
    {
        if (levelText != null) levelText.text = $"”ровень: {lvl}";
        if (expText != null) expText.text = $"ќпыт: {currentExp} / {expToNext}";
    }
}