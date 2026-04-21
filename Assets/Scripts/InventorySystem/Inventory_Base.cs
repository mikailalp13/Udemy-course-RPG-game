using System;
using UnityEngine;
using System.Collections.Generic;

public class Inventory_Base : MonoBehaviour, ISaveable
{
    protected Player player;
    public event Action OnInventoryChange;

    public int max_inventory_size = 10;
    public List<Inventory_Item> item_list = new List<Inventory_Item>();


    [Header("ITEM DATA BASE")]
    [SerializeField] protected ItemListDataSO item_data_base;



    protected virtual void Awake()
    {
        player = GetComponent<Player>();
    }

    public void TryUseItem(Inventory_Item item_to_use)
    {
        Inventory_Item consumable = item_list.Find(item => item == item_to_use);

        if (consumable == null)
            return;

        if (consumable.item_effect.CanBeUsed(player) == false)
            return;

        consumable.item_effect.ExecuteEffect();

        if (consumable.stack_size > 1)
            consumable.RemoveStack();
        else
            RemoveOneItem(consumable);

        OnInventoryChange?.Invoke();
    }

    public bool CanAddItem(Inventory_Item item_to_add)
    {
        bool has_stackable = FindStackable(item_to_add) != null;
        return has_stackable || item_list.Count < max_inventory_size;
    } 
        
    public Inventory_Item FindStackable(Inventory_Item item_to_add)
    {
        return item_list.Find(item => item.item_data == item_to_add.item_data && item.CanAddStack());
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


    public void RemoveOneItem(Inventory_Item item_to_remove)
    {
        Inventory_Item item_in_inventory = item_list.Find(item => item == item_to_remove);

        if (item_in_inventory.stack_size > 1)
            item_in_inventory.RemoveStack();
        else
            item_list.Remove(item_to_remove);


        OnInventoryChange?.Invoke();
    }


    public void RemoveFullStack(Inventory_Item item_to_remove)
    {
        for (int i = 0; i < item_to_remove.stack_size; i++)
        {
            RemoveOneItem(item_to_remove);
        }
    }

    public Inventory_Item FindItem(Inventory_Item item_to_find)
    {
        return item_list.Find(item => item == item_to_find);
    }

    public Inventory_Item FindSameItem(Inventory_Item item_to_find)
    {
        return item_list.Find(item => item.item_data == item_to_find.item_data);
    }


    public virtual void LoadData(GameData data)
    {
        
    }


    public virtual void SaveData(ref GameData data)
    {
        
    }


    public void TriggerUpdateUI() => OnInventoryChange?.Invoke();
}
