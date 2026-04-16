using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private Inventory_Storage storage;

    public enum StorageSlotType { StorageSlot, PlayerInventorySlot }

    public StorageSlotType slot_type;

    public void SetStorage(Inventory_Storage storage) => this.storage = storage;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item_in_slot == null)
            return;

        bool transfer_full_stack = Input.GetKey(KeyCode.LeftControl);

        if (slot_type == StorageSlotType.StorageSlot)
            storage.FromStorageToPlayer(item_in_slot, transfer_full_stack);
        
        if (slot_type == StorageSlotType.PlayerInventorySlot)
            storage.FromPlayerToStorage(item_in_slot, transfer_full_stack);

        ui.item_tool_tip.ShowToolTip(false, null);
    }
}
