using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] ui_stat_slots;
    private Inventory_Player inventory;

    private void Awake()
    {
        ui_stat_slots = GetComponentsInChildren<UI_StatSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateStatsUI;
    }

    private void Start()
    {
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        foreach (var stat_slot in ui_stat_slots)
            stat_slot.UpdateStatValue();
    }
}
