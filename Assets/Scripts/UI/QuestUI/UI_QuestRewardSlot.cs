using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuestRewardSlot : UI_ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (item_in_slot == null)
            return;

        ui.item_tool_tip.ShowToolTip(true, rect, item_in_slot, false, false, false);
    }
}
