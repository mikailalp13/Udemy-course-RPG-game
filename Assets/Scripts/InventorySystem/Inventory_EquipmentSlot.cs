using UnityEngine;
using System;

[Serializable]
public class Inventory_EquipmentSlot 
{
    public ItemType slot_type;
    public Inventory_Item equiped_item;


    public bool HasItem() => equiped_item != null && equiped_item.item_data != null;
}
