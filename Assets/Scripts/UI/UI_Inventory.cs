using UnityEngine;
using System.Collections.Generic;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;
    private UI_ItemSlot[] ui_item_slots;
    private UI_EquipSlot[] ui_equip_slots;

    [SerializeField] private Transform ui_item_slot_parent;
    [SerializeField] private Transform ui_equip_slot_parent;


    private void Awake()
    {
        ui_item_slots = ui_item_slot_parent.GetComponentsInChildren<UI_ItemSlot>();
        ui_equip_slots = ui_equip_slot_parent.GetComponentsInChildren<UI_EquipSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateInventorySlots();
        UpdateEquipmentSlots();
    }

    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> player_equip_list = inventory.equip_list;

        for (int i = 0; i < ui_equip_slots.Length; i++)
        {
            var player_equip_slot = player_equip_list[i];

            if (player_equip_slot.HasItem() == false)
                ui_equip_slots[i].UpdateSlot(null);
            else
                ui_equip_slots[i].UpdateSlot(player_equip_slot.equiped_item);
        }
    }

    private void UpdateInventorySlots()
    {
        List<Inventory_Item> item_list = inventory.item_list;

        for (int i = 0; i < ui_item_slots.Length; i++)
        {
            if (i < item_list.Count)
            {
                ui_item_slots[i].UpdateSlot(item_list[i]);
            }
            else
            {
                ui_item_slots[i].UpdateSlot(null);
            } 
        }
    }
}
