using UnityEngine;
using System.Collections.Generic;

public class TalentManager : MonoBehaviour
{
    public static TalentManager Instance;

    [Header("Settings")]
    public List<TalentData> allTalents; // Все таланты в игре 
    public int availablePoints = 3;     // Доступные очки талантов на старте

    private HashSet<TalentData> learnedTalents = new HashSet<TalentData>();
    private PlayerStats playerStats;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Получаем компонент характеристик с того же объекта
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("[TalentManager] PlayerStats не найден на этом объекте!");
        }
    }

    void Start()
    {
        // Повторное применение модификаторов (полезно при загрузке сохранения)
        foreach (var talent in learnedTalents)
        {
            ApplyTalent(talent);
        }
    }

    public bool CanLearn(TalentData talent)
    {
        if (learnedTalents.Contains(talent)) return false;
        if (availablePoints < talent.talentPointCost) return false;

        foreach (var prereq in talent.prerequisites)
        {
            if (!learnedTalents.Contains(prereq)) return false;
        }

        return true;
    }

    public void LearnTalent(TalentData talent)
    {
        if (!CanLearn(talent)) return;

        learnedTalents.Add(talent);
        availablePoints -= talent.talentPointCost;

        ApplyTalent(talent);

        Debug.Log($"[TalentManager] Изучен талант: {talent.displayName}. Осталось очков: {availablePoints}");
    }

    private void ApplyTalent(TalentData talent)
    {
        if (playerStats == null) return;

        foreach (var mod in talent.modifiers)
        {
            playerStats.AddModifier(mod.statName, mod.value);
        }
    }

    public bool IsLearned(TalentData talent)
    {
        return learnedTalents.Contains(talent);
    }

    public List<TalentData> GetLearnedTalents()
    {
        return new List<TalentData>(learnedTalents);
    }

    public void LoadProgress(List<TalentData> saved, int points)
    {
        learnedTalents.Clear();
        availablePoints = points;
        foreach (var t in saved)
        {
            LearnTalent(t);
        }
    }
}