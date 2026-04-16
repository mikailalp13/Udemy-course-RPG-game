using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] private UI_ItemSlotParent inventory_parent;

    private Inventory_Player inventory;
    private UI_CraftPreview craft_preview_ui;
    private UI_CraftSlot[] craft_slots;
    private UI_CraftListButton[] craft_list_buttons;


    public void SetupCraftUI(Inventory_Storage storage)
    {
        inventory = storage.player_inventory;
        inventory.OnInventoryChange += UpdateUI;
        UpdateUI();

        craft_preview_ui = GetComponentInChildren<UI_CraftPreview>();
        craft_preview_ui.SetupCraftPreview(storage);
        SetupCraftListButtons();
    }

    private void SetupCraftListButtons()
    {
        craft_slots = GetComponentsInChildren<UI_CraftSlot>();
        craft_list_buttons = GetComponentsInChildren<UI_CraftListButton>();

        foreach (var slot in craft_slots)
            slot.gameObject.SetActive(false);


        foreach (var button in craft_list_buttons)
            button.SetCraftSlots(craft_slots);
    }

    private void UpdateUI() => inventory_parent.UpdateSlots(inventory.item_list);
}
