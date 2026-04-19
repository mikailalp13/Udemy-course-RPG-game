using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickItemSlotOption : UI_ItemSlot
{
    private UI_QuickItemSlot current_quick_item_slot;


    public void SetupOption(UI_QuickItemSlot current_quick_item_slot, Inventory_Item item_to_set)
    {
        this.current_quick_item_slot = current_quick_item_slot;
        UpdateSlot(item_to_set);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        current_quick_item_slot.SetupQuickSlotItem(item_in_slot);
        ui.in_game_ui.HideQuickItemOptions();
    }
}
