using UnityEngine;
using System.Collections.Generic;

public class UI_EquipSlotParent : MonoBehaviour
{
    private UI_EquipSlot[] equip_slots;


    public void UpdateEquipmentSlots(List<Inventory_EquipmentSlot> equip_list)
    {
        if (equip_slots == null)
            equip_slots = GetComponentsInChildren<UI_EquipSlot>();

        for (int i = 0; i < equip_slots.Length; i++)
        {
            var player_equip_slot = equip_list[i];

            if (player_equip_slot.HasItem() == false)
                equip_slots[i].UpdateSlot(null);

            else
                equip_slots[i].UpdateSlot(player_equip_slot.equiped_item);
        }
    }
}
