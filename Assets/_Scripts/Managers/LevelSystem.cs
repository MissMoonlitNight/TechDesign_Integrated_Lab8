using UnityEngine;
using System; 

public class LevelSystem : MonoBehaviour
{
    [Header("Level Settings")]
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    public event Action<int, int, int> OnLevelChanged;

    public void AddExperience(int amount)
    {
        currentExp += amount;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            level++;
            expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.1f);

            if (TalentManager.Instance != null)
            {
                TalentManager.Instance.availablePoints++;
                Debug.Log($"[LevelSystem] Level up! Уровень: {level}. Очко талантов получено.");
            }
        }

        
        OnLevelChanged?.Invoke(level, currentExp, expToNextLevel);
    }
}