using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    // Событие для обновления UI
    public event Action OnInventoryChanged;

    // Событие для квестов
    public event Action<ItemData, int> OnItemAdded;

    [Header("Стартовые ресурсы")]
    public ItemData wood;
    public ItemData stone;
    public ItemData stick;

    [Header("Стартовое оружие")]
    public ItemData pistol;

    void Start()
    {
        // Выдаем стартовые ресурсы
        if (wood) AddItem(wood, 3);
        if (stone) AddItem(stone, 5);
        if (stick) AddItem(stick, 2);

        // Выдаем стартовый пистолет
        if (pistol) AddItem(pistol, 1);

        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(ItemData item, int amount)
    {
        int total = 0;
        foreach (var slot in slots)
            if (slot.item == item) total += slot.amount;
        return total >= amount;
    }

    public void RemoveItem(ItemData item, int amount)
    {
        int remaining = amount;
        for (int i = slots.Count - 1; i >= 0 && remaining > 0; i--)
        {
            if (slots[i].item == item)
            {
                int take = Mathf.Min(slots[i].amount, remaining);
                slots[i].amount -= take;
                remaining -= take;
                if (slots[i].amount <= 0) slots.RemoveAt(i);
            }
        }
        OnInventoryChanged?.Invoke();
    }

    public void AddItem(ItemData item, int amountToAdd)
    {
        Debug.Log($"[Inventory] Попытка добавить предмет: {item.itemName} x{amountToAdd}");

        int amount = amountToAdd; 

        foreach (var slot in slots)
        {
            if (slot.item == item && slot.amount < item.maxStack)
            {
                int space = item.maxStack - slot.amount;
                int add = Mathf.Min(space, amount);
                slot.amount += add;
                amount -= add;
                if (amount == 0) break;
            }
        }

        while (amount > 0)
        {
            int add = Mathf.Min(amount, item.maxStack);
            slots.Add(new InventorySlot(item, add));
            amount -= add;
        }

        OnInventoryChanged?.Invoke();

        OnItemAdded?.Invoke(item, amountToAdd);
        Debug.Log($"[Inventory] Событие OnItemAdded вызвано для {item.itemName} x{amountToAdd}");
    }
}