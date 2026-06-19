using UnityEngine;
[System.Serializable]
public class TalentModifier
{
    public string statName; // "Health", "Damage", "Speed", "Mana"
    public float value;
}

[CreateAssetMenu(fileName = "NewTalent", menuName = "Talents/TalentData")]
public class TalentData : ScriptableObject
{
    public string displayName;
    public Sprite icon;

    [TextArea(2, 3)]
    public string description;

    public TalentModifier[] modifiers;
    public TalentData[] prerequisites; 
    public int talentPointCost = 1;    
}