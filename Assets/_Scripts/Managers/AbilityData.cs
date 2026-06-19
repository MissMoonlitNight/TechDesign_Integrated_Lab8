using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/AbilityData")]
public class AbilityData : ScriptableObject
{
    [Header("Основная информация")]
    public string abilityName = "New Ability";
    public Sprite icon;

    [Header("Характеристики")]
    public float cooldown = 3f;       // Время восстановления (сек)
    public float manaCost = 10f;      // Стоимость маны
    public float damage = 20f;        // Базовый урон
    public float range = 10f;         // Дальность применения

    [Header("Визуал и звук")]
    public GameObject effectPrefab;   // Префаб эффекта (снаряд или мгновенный эффект)
    public AudioClip castSound;       // Звук применения
}