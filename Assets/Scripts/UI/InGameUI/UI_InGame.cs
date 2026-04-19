using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    private Player player;
    private Inventory_Player inventory;
    private UI_SkillSlot[] skill_slots;


    [SerializeField] private RectTransform health_rect;
    [SerializeField] private Slider health_slider;
    [SerializeField] private TextMeshProUGUI health_text;


    [Header("Quick Item Slots")]
    [SerializeField] private float y_offset_quick_item_parent = 150f;
    [SerializeField] private Transform quick_item_options_parent;
    private UI_QuickItemSlotOption[] quick_item_options;
    private UI_QuickItemSlot[] quick_item_slots; 



    private void Start()
    {
        quick_item_slots = GetComponentsInChildren<UI_QuickItemSlot>();

        player = FindFirstObjectByType<Player>();
        player.health.OnHealthUpdate += UpdateHealthBar;

        inventory = player.inventory;
        inventory.OnInventoryChange += UpdateQuickSlotsUI;
        inventory.OnQuickSlotUsed += PlayQuickSlotFeedback;
    }


    public void PlayQuickSlotFeedback(int slot_number) => quick_item_slots[slot_number].SimulateButtonFeedback();


    public void UpdateQuickSlotsUI()
    {
        Inventory_Item[] quick_items = inventory.quick_items;

        for (int i = 0; i < quick_items.Length; i++)
            quick_item_slots[i].UpdateQuickSlotUI(quick_items[i]);
    }


    public void OpenQuickItemOptions(UI_QuickItemSlot quick_item_slot, RectTransform target_rect)
    {
        if (quick_item_options == null)
            quick_item_options = quick_item_options_parent.GetComponentsInChildren<UI_QuickItemSlotOption>(true);

        List<Inventory_Item> consumables = inventory.item_list.FindAll(item => item.item_data.item_type == ItemType.Consumable);

        for (int i = 0; i < quick_item_options.Length; i++)
        {
            if (i < consumables.Count)
            {
                quick_item_options[i].gameObject.SetActive(true);
                quick_item_options[i].SetupOption(quick_item_slot, consumables[i]);
            }
            else
                quick_item_options[i].gameObject.SetActive(false);
        }

        quick_item_options_parent.position = target_rect.position + Vector3.up * y_offset_quick_item_parent;
    }


    public void HideQuickItemOptions() => quick_item_options_parent.position = new Vector3(0, 99999);


    public UI_SkillSlot GetSkillSlot(SkillType skill_type)
    {
        if (skill_slots == null)
            skill_slots = GetComponentsInChildren<UI_SkillSlot>(true);

        foreach (var slot in skill_slots)
        {
            if (slot.skill_type == skill_type)
            {
                slot.gameObject.SetActive(true);
                return slot;
            }
        }
        
        return null;
    }

    private void UpdateHealthBar()
    {
        float current_health = Mathf.RoundToInt(player.health.GetCurrentHealth());
        float max_health = player.stats.GetMaxHealth();
        float size_difference = Mathf.Abs(max_health - health_rect.sizeDelta.x);

        if (size_difference > 0.1f)
            health_rect.sizeDelta = new Vector2(max_health, health_rect.sizeDelta.y);

        health_text.text = current_health + "/" + max_health;
        health_slider.value = player.health.GetHealthPercent();
    }
}
