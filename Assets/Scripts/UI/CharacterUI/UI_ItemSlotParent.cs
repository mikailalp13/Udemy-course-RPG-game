using UnityEngine;
using System.Collections.Generic;

public class UI_ItemSlotParent : MonoBehaviour
{
    private UI_ItemSlot[] slots;

    public void UpdateSlots(List<Inventory_Item> item_list)
    {
        if (slots == null)
            slots = GetComponentsInChildren<UI_ItemSlot>();
        
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < item_list.Count)
            {
                slots[i].UpdateSlot(item_list[i]);
            }
            else
            {
                slots[i].UpdateSlot(null);
            } 
        }
    }
}
