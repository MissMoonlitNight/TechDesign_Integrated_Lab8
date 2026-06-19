using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [Header("Level Settings")]
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    public void AddExperience(int amount)
    {
        currentExp += amount;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            level++;
            expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.1f);

            // Начисляем очко талантов через менеджер
            if (TalentManager.Instance != null)
            {
                TalentManager.Instance.availablePoints++;
                Debug.Log($"[LevelSystem] Level up! Уровень: {level}. Получено очко талантов.");
            }
            else
            {
                Debug.LogWarning("[LevelSystem] TalentManager не найден! Очко талантов не выдано.");
            }
        }
    }
}