using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    public Inventory inventory;
    public GameObject slotPrefab;
    public Transform slotContainer;

    void Start()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged += RefreshUI;
        }
        RefreshUI();
    }

    // ВАЖНО: метод должен быть public, чтобы UIManager мог его вызывать
    public void RefreshUI()
    {
        // Защита от пустых ссылок
        if (slotContainer == null || slotPrefab == null || inventory == null)
        {
            Debug.LogWarning("InventoryUI: Не назначены ссылки в Inspector!");
            return;
        }

        // Очищаем старые слоты
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        // Создаём новые слоты
        foreach (var slot in inventory.slots)
        {
            if (slot.item == null) continue;

            GameObject slotObj = Instantiate(slotPrefab, slotContainer);

            // Ищем Image для иконки
            Transform iconTransform = slotObj.transform.Find("IconImage");
            Image iconImg = iconTransform != null ? iconTransform.GetComponent<Image>() : null;

            if (iconImg != null && slot.item.icon != null)
            {
                iconImg.sprite = slot.item.icon;
                iconImg.enabled = true;
                iconImg.color = Color.white;
            }
            else if (iconImg != null)
            {
                iconImg.enabled = false;
            }

            // Ищем Text для количества по имени
            Transform textTransform = slotObj.transform.Find("AmountText");
            Text amountText = textTransform != null ? textTransform.GetComponent<Text>() : null;

            if (amountText != null)
            {
                amountText.text = slot.amount.ToString();
            }
        }
    }
}