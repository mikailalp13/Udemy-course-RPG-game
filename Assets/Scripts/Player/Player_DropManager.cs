using System.Collections.Generic;
using UnityEngine;

public class Player_DropManager : Entity_DropManager
{
    [Header("Player Drop Details")]
    [Range(0, 100)]
    [SerializeField] private float chance_to_loose_item = 90f;
    private Inventory_Player inventory;


    private void Awake()
    {
        inventory = GetComponent<Inventory_Player>();
    }

    public override void DropItems()
    {
        List<Inventory_Item> inventory_copy = new List<Inventory_Item>(inventory.item_list);
        List<Inventory_EquipmentSlot> equip_copy = new List<Inventory_EquipmentSlot>(inventory.equip_list);

        foreach (var item in inventory_copy)
        {
            if (Random.Range(0, 100) < chance_to_loose_item)
            {
                CreateItemDrop(item.item_data);
                inventory.RemoveFullStack(item);
            }
        }

        foreach (var equip in equip_copy)
        {
            if (Random.Range(0, 100) < chance_to_loose_item && equip.HasItem())
            {
                var item = equip.GetEquipedItem();

                CreateItemDrop(item.item_data);
                inventory.UnequipItem(item);
                inventory.RemoveFullStack(item);
            }
        }
    }

}
