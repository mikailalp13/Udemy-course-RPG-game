using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipSlot : UI_ItemSlot
{
    public ItemType slot_type;

    private void OnValidate()
    {
        gameObject.name = "UI_EquipmentSlot - " + slot_type.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item_in_slot == null)
            return;

        inventory.UnequipItem(item_in_slot);
    }
}
