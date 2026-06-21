using UnityEngine;
using System.Collections.Generic;

public class CraftingSystem : MonoBehaviour
{
    public Inventory playerInventory;
    public List<RecipeData> availableRecipes; // все рецепты

    // Словарь для связи результата крафта с WeaponData
    [System.Serializable]
    public class WeaponCraftResult
    {
        public ItemData resultItem;
        public WeaponData weaponData;
    }

    public List<WeaponCraftResult> weaponResults = new List<WeaponCraftResult>();

    public bool CanCraft(RecipeData recipe)
    {
        foreach (var ing in recipe.ingredients)
        {
            if (!playerInventory.HasItem(ing.item, ing.amount))
                return false;
        }
        return true;
    }

    public void Craft(RecipeData recipe)
    {
        if (!CanCraft(recipe)) return;

        // Списываем ингредиенты
        foreach (var ing in recipe.ingredients)
        {
            playerInventory.RemoveItem(ing.item, ing.amount);
        }

        // Добавляем результат
        playerInventory.AddItem(recipe.result, recipe.resultAmount);

        Debug.Log($"[CraftingSystem] Скрафчен: {recipe.recipeName}");
    }

    public WeaponData GetWeaponDataForItem(ItemData item)
    {
        foreach (var wr in weaponResults)
        {
            if (wr.resultItem == item)
                return wr.weaponData;
        }
        return null;
    }
}