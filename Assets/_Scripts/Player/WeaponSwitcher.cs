using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Базовое оружие (в иерархии)")]
    public Weapon[] weapons;
    private int currentIndex = 0;

    [Header("Инвентарь и крафт")]
    public Inventory inventory;
    public CraftingSystem craftingSystem;

    [Header("UI")]
    public Text ammoText;

    [Header("Настройки для крафта")]
    public Transform weaponHolder;      
    public GameObject baseWeaponModel;  

    private Dictionary<ItemData, Weapon> weaponMap = new Dictionary<ItemData, Weapon>();

    void Start()
    {
        // Активируем только первое оружие, остальные скрываем
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == 0);
            weapons[i].ammoText = ammoText;
        }

        if (inventory != null)
        {
            inventory.OnInventoryChanged += CheckForNewWeapons;
        }
    }

    void CheckForNewWeapons()
    {
        if (inventory == null || craftingSystem == null) return;

        foreach (var slot in inventory.slots)
        {
            if (slot.item == null) continue;

            if (!weaponMap.ContainsKey(slot.item))
            {
                WeaponData weaponData = craftingSystem.GetWeaponDataForItem(slot.item);
                if (weaponData != null)
                {
                    AddWeaponToSwitcher(weaponData, slot.item);
                }
            }
        }
    }

    void AddWeaponToSwitcher(WeaponData weaponData, ItemData itemData)
    {
        if (weaponHolder == null || baseWeaponModel == null)
        {
            Debug.LogError("[WeaponSwitcher] Не назначены WeaponHolder или BaseWeaponModel в Inspector!");
            return;
        }

        // Создаём визуальную копию базового оружия 
        GameObject weaponObj = Instantiate(baseWeaponModel, weaponHolder);
        weaponObj.name = weaponData.weaponName;

        weaponObj.transform.localPosition = Vector3.zero;
        weaponObj.transform.localRotation = Quaternion.identity;

        // Красим в серый цвет
        Renderer renderer = weaponObj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.grey;
        }

        Weapon newWeapon = weaponObj.AddComponent<Weapon>();
        newWeapon.data = weaponData;
        newWeapon.ammoText = ammoText;

        GameObject firePoint = new GameObject("FirePoint");
        firePoint.transform.SetParent(weaponObj.transform);
        firePoint.transform.localPosition = new Vector3(0, 0, 0.5f); 
        newWeapon.firePoint = firePoint.transform;

        List<Weapon> weaponList = new List<Weapon>(weapons);
        weaponList.Add(newWeapon);
        weapons = weaponList.ToArray();

        weaponMap[itemData] = newWeapon;
        newWeapon.gameObject.SetActive(false); 

        Debug.Log($"[WeaponSwitcher] Скрафчено и добавлено оружие: {weaponData.weaponName}");
    }
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int newIndex = currentIndex + (scroll > 0 ? 1 : -1);
            if (newIndex < 0) newIndex = weapons.Length - 1;
            if (newIndex >= weapons.Length) newIndex = 0;
            SwitchTo(newIndex);
        }
    }

    void SwitchTo(int index)
    {
        if (index == currentIndex || index >= weapons.Length) return;

        weapons[currentIndex].gameObject.SetActive(false);
        weapons[index].gameObject.SetActive(true);
        currentIndex = index;
    }
}