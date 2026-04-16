using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private Inventory_Storage storage;
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent inventory_parent;
    [SerializeField] private UI_ItemSlotParent storage_parent;
    [SerializeField] private UI_ItemSlotParent material_stash_parent; 

    public void SetupStorageUI(Inventory_Storage storage)
    {
        this.storage = storage;
        inventory = storage.player_inventory;

        storage.OnInventoryChange += UpdateUI;
        UpdateUI();

        UI_StorageSlot[] storage_slots = GetComponentsInChildren<UI_StorageSlot>();

        foreach (var slot in storage_slots)
            slot.SetStorage(storage);
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (storage == null)
            return;
            
        inventory_parent.UpdateSlots(inventory.item_list);
        storage_parent.UpdateSlots(storage.item_list);
        material_stash_parent.UpdateSlots(storage.material_stash);
    }
}
