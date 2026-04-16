using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
    private Inventory_Item item_to_craft;
    private Inventory_Storage storage;
    private UI_CraftPreviewSlot[] craft_preview_slots; 


    [Header("Item Preview Setup")]
    [SerializeField] private Image item_icon;
    [SerializeField] private TextMeshProUGUI item_name;
    [SerializeField] private TextMeshProUGUI item_info;
    [SerializeField] private TextMeshProUGUI button_text;


    public void SetupCraftPreview(Inventory_Storage storage)
    {
        this.storage = storage;

        craft_preview_slots = GetComponentsInChildren<UI_CraftPreviewSlot>();

        foreach (var slot in craft_preview_slots)
            slot.gameObject.SetActive(false);
    }

    public void ConfirmCraft()
    {
        if (item_to_craft == null)
        {
            button_text.text = "Pick an item.";
            return;
        }

        if (storage.CanCraftItem(item_to_craft))
            storage.CraftItem(item_to_craft);

        UpdateCraftPreviewSlots();
    }

    public void UpdateCraftPreview(ItemDataSO item_data)
    {
        item_to_craft = new Inventory_Item(item_data);

        item_icon.sprite = item_data.item_icon;
        item_name.text = item_data.item_name;
        item_info.text = item_to_craft.GetItemInfo();
        UpdateCraftPreviewSlots();
    }

    private void UpdateCraftPreviewSlots(){

        foreach (var slot in craft_preview_slots)
            slot.gameObject.SetActive(false);
        
        for (int i = 0; i < item_to_craft.item_data.craft_recipe.Length; i++)
        {
            Inventory_Item required_item = item_to_craft.item_data.craft_recipe[i];
            int avaliable_amount = storage.GetAvailableAmountOf(required_item.item_data);
            int required_amount = required_item.stack_size;

            craft_preview_slots[i].gameObject.SetActive(true);
            craft_preview_slots[i].SetupPreviewSlot(required_item.item_data, avaliable_amount, required_amount);
        }   
    }
}
