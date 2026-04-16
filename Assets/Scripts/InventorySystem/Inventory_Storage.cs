using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Inventory_Storage : Inventory_Base
{
    public Inventory_Player player_inventory { get; private set; }
    public List<Inventory_Item> material_stash;


    public void CraftItem(Inventory_Item item_to_craft)
    {
        ConsumeMaterials(item_to_craft);
        player_inventory.AddItem(item_to_craft);
    }


    public bool CanCraftItem(Inventory_Item item_to_craft)
    {
        return HasEnoughMaterials(item_to_craft) && player_inventory.CanAddItem(item_to_craft);
    }


    private void ConsumeMaterials(Inventory_Item item_to_craft)
    {
        foreach (var required_item in item_to_craft.item_data.craft_recipe)
        {
            int amount_to_consume = required_item.stack_size;

            amount_to_consume = amount_to_consume - ConsumedMaterailsAmount(player_inventory.item_list, required_item);

            if (amount_to_consume > 0)
                amount_to_consume = amount_to_consume - ConsumedMaterailsAmount(item_list, required_item);

            if (amount_to_consume > 0)
                amount_to_consume = amount_to_consume - ConsumedMaterailsAmount(material_stash, required_item);
        }
    }


    private int ConsumedMaterailsAmount(List<Inventory_Item> item_list, Inventory_Item needed_item)
    {
        int amount_needed = needed_item.stack_size;
        int consumed_amount = 0;

        foreach (var item in item_list)
        {
            if (item.item_data != needed_item.item_data)
                continue;
            
            int remove_amount = Mathf.Min(item.stack_size, amount_needed - consumed_amount);

            item.stack_size = item.stack_size - remove_amount;
            consumed_amount = consumed_amount + remove_amount;

            if (item.stack_size <= 0)
                item_list.Remove(item);
            
            if (consumed_amount >= amount_needed)
                break;
        }

        return consumed_amount;
    }


    private bool HasEnoughMaterials(Inventory_Item item_to_craft)
    {
        foreach (var required_material in item_to_craft.item_data.craft_recipe)
        {
            if (GetAvailableAmountOf(required_material.item_data) < required_material.stack_size)
                return false;
        }

        return true;
    }


    public int GetAvailableAmountOf(ItemDataSO required_item)
    {
        int amount = 0;

        foreach (var item in player_inventory.item_list)
        {
            if (item.item_data == required_item)
                amount = amount + item.stack_size;
        }

        foreach (var item in item_list)
        {
            if (item.item_data == required_item)
                amount = amount + item.stack_size;
        }

        foreach (var item in material_stash)
        {
            if (item.item_data == required_item)
                amount = amount + item.stack_size;
        }

        return amount;
    }


    public void AddMaterialToStash(Inventory_Item item_to_add)
    {
        var stackable_item = StackableInStash(item_to_add);

        if (stackable_item != null)
            stackable_item.AddStack();
        else
        {
            var new_item_to_add = new Inventory_Item(item_to_add.item_data);
            material_stash.Add(new_item_to_add);
        }

        TriggerUpdateUI();
        material_stash = material_stash.OrderBy(item => item.item_data.name).ToList();
    }


    public Inventory_Item StackableInStash(Inventory_Item item_to_add)
    {
        List<Inventory_Item> stackable_items = material_stash.FindAll(item => item.item_data == item_to_add.item_data);

        foreach (var stackable in stackable_items)
        {
            if (stackable.CanAddStack())
                return stackable;
        }

        return null;
    } 


    public void SetInventory(Inventory_Player inventory) => this.player_inventory = inventory;


    public void FromPlayerToStorage(Inventory_Item item, bool transfer_full_stack)
    {
        int transfer_amount = transfer_full_stack ? item.stack_size : 1;

        for (int i = 0; i < transfer_amount; i++)
        {
            if (CanAddItem(item))
            {
                var item_to_add = new Inventory_Item(item.item_data);

                player_inventory.RemoveOneItem(item);
                AddItem(item_to_add);
            }
        }

        TriggerUpdateUI();
    }


    public void FromStorageToPlayer(Inventory_Item item, bool transfer_full_stack)
    {
        int transfer_amount = transfer_full_stack ? item.stack_size : 1;

        for (int i = 0; i < transfer_amount; i++)
        {
            if (player_inventory.CanAddItem(item))
            {
                var item_to_add = new Inventory_Item(item.item_data);

                RemoveOneItem(item);
                player_inventory.AddItem(item_to_add);
            }
        }

        TriggerUpdateUI();
    }
}
