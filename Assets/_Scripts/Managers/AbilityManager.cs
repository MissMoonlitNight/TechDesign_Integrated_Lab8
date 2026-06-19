using UnityEngine;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour
{
    public List<AbilityData> learnedAbilities = new List<AbilityData>();
    private Dictionary<AbilityData, float> cooldowns = new Dictionary<AbilityData, float>();

    private PlayerStats playerStats; 

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        foreach (var ability in learnedAbilities)
        {
            cooldowns[ability] = 0f;
        }
    }

    void Update()
    {
        var keys = new List<AbilityData>(cooldowns.Keys);
        foreach (var ability in keys)
        {
            if (cooldowns[ability] > 0)
                cooldowns[ability] -= Time.deltaTime;
        }
    }

    public bool CanCast(AbilityData ability)
    {
        if (!learnedAbilities.Contains(ability)) return false;
        if (cooldowns[ability] > 0) return false;

        if (playerStats != null && playerStats.currentMana < ability.manaCost)
            return false;

        return true;
    }

    public void CastAbility(AbilityData ability, Vector3 targetPoint)
    {
        if (!CanCast(ability)) return;

        if (playerStats != null)
        {
            playerStats.SpendMana(ability.manaCost);
        }

        // Устанавливаем кулдаун
        cooldowns[ability] = ability.cooldown;

        // Запускаем эффект
        if (ability.effectPrefab != null)
        {
            Vector3 spawnPoint = transform.position + transform.forward * 1.5f + Vector3.up * 0.6f;
            GameObject effect = Instantiate(ability.effectPrefab, spawnPoint, transform.rotation);
            AbilityEffect effectComp = effect.GetComponent<AbilityEffect>();
            if (effectComp != null)
            {
                effectComp.Initialize(ability, targetPoint, gameObject);
            }
        }
        else
        {
            // Fallback: прямой урон по цели под курсором
            RaycastHit hit;
            Vector3 dir = (targetPoint - transform.position).normalized;
            if (Physics.Raycast(transform.position, dir, out hit, ability.range))
            {
               
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.TakeDamage(ability.damage);
            }
        }

        if (ability.castSound != null)
            AudioSource.PlayClipAtPoint(ability.castSound, transform.position);
    }

    public float GetCooldownRemaining(AbilityData ability)
    {
        return cooldowns.ContainsKey(ability) ? cooldowns[ability] : 0f;
    }
}
