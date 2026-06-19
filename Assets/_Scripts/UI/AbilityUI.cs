using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    public AbilityManager abilityManager;
    public Transform slotContainer;
    public GameObject slotPrefab;

    private Text[] cooldownTexts;
    private AbilityData[] abilities;

    void Start()
    {
        abilities = abilityManager.learnedAbilities.ToArray();
        cooldownTexts = new Text[abilities.Length];

        for (int i = 0; i < abilities.Length; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);

            // Устанавливаем иконку способности
            Image icon = slot.transform.Find("Icon").GetComponent<Image>();
            icon.sprite = abilities[i].icon;

            // Получаем текст кулдауна
            cooldownTexts[i] = slot.transform.Find("CooldownText").GetComponent<Text>();
        }
    }

    void Update()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            // Отображение кулдауна
            float remaining = abilityManager.GetCooldownRemaining(abilities[i]);
            if (remaining > 0)
            {
                cooldownTexts[i].text = Mathf.Ceil(remaining).ToString();
            }
            else
            {
                cooldownTexts[i].text = "";
            }

            // Клавиши 1, 2, 3
            KeyCode key = KeyCode.Alpha1 + i;
            if (Input.GetKeyDown(key))
            {
                Vector3 target = GetMouseWorldPosition();
                abilityManager.CastAbility(abilities[i], target);
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}