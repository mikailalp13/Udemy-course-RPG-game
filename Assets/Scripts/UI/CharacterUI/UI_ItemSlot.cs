using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory_Item item_in_slot { get; private set; }
    protected Inventory_Player inventory;
    protected UI ui;
    protected RectTransform rect;


    [Header("UI Slot Setup")]
    [SerializeField] private GameObject default_icon;
    [SerializeField] protected Image item_icon; 
    [SerializeField] protected TextMeshProUGUI item_stack_size;


    protected virtual void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        inventory = FindAnyObjectByType<Inventory_Player>();    
    }


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item_in_slot == null || item_in_slot.item_data.item_type == ItemType.Material)
            return;

        bool alternative_input = Input.GetKey(KeyCode.LeftControl);

        if (alternative_input)
        {
            inventory.RemoveOneItem(item_in_slot);
        }
        else
        {
            if (item_in_slot.item_data.item_type == ItemType.Consumable)
            {
                inventory.TryUseItem(item_in_slot);
            }
            else
                inventory.TryEquipItem(item_in_slot);
        }
        
        if (item_in_slot == null)
            ui.item_tool_tip.ShowToolTip(false, null);        
    }

    public void UpdateSlot(Inventory_Item item)
    {
        item_in_slot = item;

        if (default_icon != null)
            default_icon.gameObject.SetActive(item_in_slot == null);

        if (item_in_slot == null)
        {
            item_stack_size.text = "";
            item_icon.color = Color.clear;
            return;
        }

        Color color = Color.white; color.a = 0.9f;
        item_icon.color = color;
        item_icon.sprite = item_in_slot.item_data.item_icon;
        item_stack_size.text = item.stack_size > 1 ? item.stack_size.ToString() : "";
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (item_in_slot == null)
            return;

        ui.item_tool_tip.ShowToolTip(true, rect, item_in_slot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.item_tool_tip.ShowToolTip(false, null);
    }
}
