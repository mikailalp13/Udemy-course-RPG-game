using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Merchant merchant;
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent merchant_slots;
    [SerializeField] private UI_ItemSlotParent inventory_slots;
    [SerializeField] private UI_EquipSlotParent equip_slots;


    public void SetupMerchantUI(Inventory_Merchant merchant, Inventory_Player inventory)
    {
        this.merchant = merchant;
        this.inventory = inventory;

        this.inventory.OnInventoryChange += UpdateSlotUI;
        this.merchant.OnInventoryChange += UpdateSlotUI;
        UpdateSlotUI();

        UI_MerchantSlot[] merchant_slots = GetComponentsInChildren<UI_MerchantSlot>();

        foreach (var slot in merchant_slots)
            slot.SetupMerchantUI(merchant);
    }

    private void UpdateSlotUI()
    {
        if (inventory == null)
            return;
        
        merchant_slots.UpdateSlots(merchant.item_list);
        inventory_slots.UpdateSlots(inventory.item_list);
        equip_slots.UpdateEquipmentSlots(inventory.equip_list);
    }
}
