using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory_Player : Inventory_Base
{
    public event Action<int> OnQuickSlotUsed;

    public Inventory_Storage storage { get; private set; }
    public List<Inventory_EquipmentSlot> equip_list;


    [Header("Quick Item Slots")]
    public Inventory_Item[] quick_items = new Inventory_Item[2];


    [Header("Gold Info")]
    public int gold = 1000;



    protected override void Awake()
    {
        base.Awake();

        storage = FindFirstObjectByType<Inventory_Storage>();
    }


    public void SetQuickItemInSlot(int slot_number, Inventory_Item item_to_set)
    {
        quick_items[slot_number - 1] = item_to_set;
        TriggerUpdateUI();
    }


    public void TryUseQuickItemInSlot(int passed_slot_number)
    {
        int slot_number = passed_slot_number - 1;
        var item_to_use = quick_items[slot_number];

        if (item_to_use == null)
            return;
        
        TryUseItem(item_to_use);

        if (FindItem(item_to_use) == null)
        {
            quick_items[slot_number] = FindSameItem(item_to_use);
        }

        TriggerUpdateUI();
        OnQuickSlotUsed?.Invoke(slot_number);
    }


    public void TryEquipItem(Inventory_Item item)
    {
        var inventory_item = FindItem(item);
        var matching_slots = equip_list.FindAll(slot => slot.slot_type == item.item_data.item_type);

        // Step 1: Try to find an empty slot and equip item
        foreach (var slot in matching_slots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(inventory_item, slot);
                return;
            }
        }

        // Step 2: No empty slots? replace the first one
        var slot_to_replace = matching_slots[0];
        var item_to_unequip = slot_to_replace.equiped_item;

        UnequipItem(item_to_unequip, slot_to_replace != null);
        EquipItem(inventory_item, slot_to_replace);
    }


    private void EquipItem(Inventory_Item item_to_equip, Inventory_EquipmentSlot slot)
    {
        float saved_health_percent = player.health.GetHealthPercent();

        slot.equiped_item = item_to_equip;
        slot.equiped_item.AddModifiers(player.stats);
        slot.equiped_item.AddItemEffect(player);

        player.health.SetHealthToPercent(saved_health_percent);
        RemoveOneItem(item_to_equip);
    }

    public void UnequipItem(Inventory_Item item_to_unequip, bool replacing_item = false)
    {
        if (CanAddItem(item_to_unequip) == false && replacing_item == false)
        {
            Debug.Log("No space!");
            return;
        }

        float saved_health_percent = player.health.GetHealthPercent();
        var slot_to_unequip = equip_list.Find(slot => slot.equiped_item == item_to_unequip);

        if (slot_to_unequip != null)
            slot_to_unequip.equiped_item = null;

        item_to_unequip.RemoveModifiers(player.stats);
        item_to_unequip.RemoveItemEffect(player);

        player.health.SetHealthToPercent(saved_health_percent);
        AddItem(item_to_unequip);
    }

    public override void SaveData(ref GameData data)
    {
        data.gold = gold;
        data.inventory.Clear();
        data.equiped_items.Clear();

        foreach (var item in item_list)
        {
            if (item != null && item.item_data != null)
            {
                string save_id = item.item_data.save_id;

                if (data.inventory.ContainsKey(save_id) == false)
                    data.inventory[save_id] = 0;

                data.inventory[save_id] += item.stack_size;
            }
        }

        foreach (var slot in equip_list)
        {
            if (slot.HasItem())
                data.equiped_items[slot.equiped_item.item_data.save_id] = slot.slot_type;
        }
    }

    public override void LoadData(GameData data)
    {
        gold = data.gold;

        foreach (var entry in data.inventory)
        {
            string save_id = entry.Key;
            int stack_size = entry.Value;

            ItemDataSO item_data = item_data_base.GetItemData(save_id);

            if (item_data == null)
            {
                Debug.LogWarning("Item not found: " + save_id);
                continue;
            }

            for (int i = 0; i < stack_size; i++)
            {
                Inventory_Item item_to_load = new Inventory_Item(item_data);
                AddItem(item_to_load);
            }
        }

        foreach (var entry in data.equiped_items)
        {
            string save_id = entry.Key;
            ItemType loaded_slot_type = entry.Value;

            ItemDataSO item_data = item_data_base.GetItemData(save_id);
            Inventory_Item item_to_load = new Inventory_Item(item_data);

            var slot = equip_list.Find(slot => slot.slot_type == loaded_slot_type && slot.HasItem() == false);

            slot.equiped_item = item_to_load;
            slot.equiped_item.AddModifiers(player.stats);
            slot.equiped_item.AddItemEffect(player);
        }

        TriggerUpdateUI();
    }
}
