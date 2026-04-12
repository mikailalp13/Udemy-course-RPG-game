using UnityEngine;
using System;

[Serializable]
public class Inventory_Item 
{
    private string item_id;

    public ItemDataSO item_data;
    public int stack_size = 1;

    public ItemModifier[] modifiers { get; private set; }
    public ItemEffectDataSO item_effect;

    public Inventory_Item(ItemDataSO item_data)
    {
        this.item_data = item_data;
        item_effect = item_data.item_effect;
        modifiers = EquipmentData()?.modifiers;

        item_id = item_data.item_name + " - " + Guid.NewGuid();
    }

    public void AddModifiers(Entity_Stats player_stats)
    {
        foreach (var mod in modifiers)
        {
            Stat stat_to_modify = player_stats.GetStatByType(mod.stat_type);
            stat_to_modify.AddModifier(mod.value, item_id);
        }
    }

    public void RemoveModifiers(Entity_Stats player_stats)
    {
        foreach (var mod in modifiers)
        {
            Stat stat_to_modify = player_stats.GetStatByType(mod.stat_type);
            stat_to_modify.RemoveModifier(item_id);
        }
    }

    private EquipmentDataSO EquipmentData()
    {
        if (item_data is EquipmentDataSO equipment)
            return equipment;
        
        return null;
    }

    public void AddItemEffect(Player player) => item_effect?.Subscribe(player);
    public void RemoveItemEffect(Player player) => item_effect?.Unsubscribe();

    public bool CanAddStack() => stack_size < item_data.max_stack_size;
    public void AddStack() => stack_size++;   
    public void RemoveStack() => stack_size--;
}
