using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    private Inventory_Merchant merchant;
    public enum MerchantSlotType { MerchantSlot, PlayerSlot }
    public MerchantSlotType slot_type;


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item_in_slot == null)
            return;
        
        bool right_button = eventData.button == PointerEventData.InputButton.Right;
        bool left_button = eventData.button == PointerEventData.InputButton.Left;

        if (slot_type == MerchantSlotType.PlayerSlot)
        {
            if (right_button)
            {
                bool sell_full_stack = Input.GetKey(KeyCode.LeftControl);
                merchant.TrySellItem(item_in_slot, sell_full_stack);
            }
            else if (left_button)
            {
                base.OnPointerDown(eventData);
            }
        }
        else if (slot_type == MerchantSlotType.MerchantSlot)
        {
            if (left_button)
                return;
            
            bool buy_full_stack = Input.GetKey(KeyCode.LeftControl);
            merchant.TryBuyItem(item_in_slot, buy_full_stack);
        }

        ui.item_tool_tip.ShowToolTip(false, null);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (item_in_slot == null)
            return;
        
        if (slot_type == MerchantSlotType.MerchantSlot)
            ui.item_tool_tip.ShowToolTip(true, rect, item_in_slot, true, true);
        else
            ui.item_tool_tip.ShowToolTip(true, rect, item_in_slot, false, true);
    }

    public void SetupMerchantUI(Inventory_Merchant merchant) => this.merchant = merchant;
}
