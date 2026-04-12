using UnityEngine;
using System.Collections.Generic;
using System;
public class Inventory_Base : MonoBehaviour
{
    public event Action OnInventoryChange;

    public int max_inventory_size = 10;
    public List<Inventory_Item> item_list = new List<Inventory_Item>();


    protected virtual void Awake()
    {
        
    }

    public void TryUseItem(Inventory_Item item_to_use)
    {
        Inventory_Item consumable = item_list.Find(item => item == item_to_use);

        if (consumable == null)
            return;

        consumable.item_effect.ExecuteEffect();

        if (consumable.stack_size > 1)
            consumable.RemoveStack();
        else
            RemoveItem(consumable);

        OnInventoryChange?.Invoke();
    }

    public bool CanAddItem() => item_list.Count < max_inventory_size;
    public Inventory_Item FindStackable(Inventory_Item item_to_add)
    {
        List<Inventory_Item> stackable_items = item_list.FindAll(item => item.item_data == item_to_add.item_data);

        foreach (var stackable_item in stackable_items)
        {
            if (stackable_item.CanAddStack())
                return stackable_item;
        }

        return null;
    } 

    public void AddItem(Inventory_Item item_to_add)
    {
        Inventory_Item item_in_inventory = FindStackable(item_to_add);

        if (item_in_inventory != null)
            item_in_inventory.AddStack();
        else
            item_list.Add(item_to_add);

        OnInventoryChange?.Invoke();
    }


    public void RemoveItem(Inventory_Item item_to_remove)
    {
        item_list.Remove(item_to_remove);
        OnInventoryChange?.Invoke();
    }

    public Inventory_Item FindItem(ItemDataSO item_data)
    {
        return item_list.Find(item => item.item_data == item_data);
    }

    public void TriggerUpdateUI() => OnInventoryChange?.Invoke();
}
