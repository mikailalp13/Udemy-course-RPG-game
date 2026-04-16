using UnityEngine;
using System.Collections.Generic;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent inventory_slots_parent;
    [SerializeField] private UI_EquipSlotParent equip_slot_parent;


    private void Awake()
    {
        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        inventory_slots_parent.UpdateSlots(inventory.item_list);
        equip_slot_parent.UpdateEquipmentSlots(inventory.equip_list);
    }
}
