using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_QuickItemSlot : UI_ItemSlot
{
    private Button button;
    [SerializeField] private Sprite default_sprite;
    [SerializeField] private int slot_number;



    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }


    public void SetupQuickSlotItem(Inventory_Item item_to_pass)
    {
        inventory.SetQuickItemInSlot(slot_number, item_to_pass);
    }


    // when you press 1 and 2 on your keyboard, it will execute the same effect when you pressed button with mouse
    public void SimulateButtonFeedback()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
        ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }


    public void UpdateQuickSlotUI(Inventory_Item current_item_in_slot)
    {
        if (current_item_in_slot == null || current_item_in_slot.item_data == null)
        {
            item_icon.sprite = default_sprite;
            item_stack_size.text = "";
            return;
        }
        
        item_icon.sprite = current_item_in_slot.item_data.item_icon;
        item_stack_size.text = current_item_in_slot.stack_size.ToString();
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.in_game_ui.OpenQuickItemOptions(this, rect);
    }
}
