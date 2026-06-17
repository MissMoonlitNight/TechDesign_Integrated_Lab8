using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    public int amount = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inv = FindObjectOfType<Inventory>();
            if (inv != null && item != null)
            {
                inv.AddItem(item, amount);
                Debug.Log($"[ItemPickup] Подобрано: {item.itemName} x{amount}");
                Destroy(gameObject);
            }
        }
    }
}