using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skill_tool_tip { get; private set; }
    public UI_ItemToolTip item_tool_tip { get; private set; }
    public UI_StatToolTip stat_tool_tip { get; private set; }

    public UI_SkillTree skill_tree_ui { get; private set; }
    public UI_Inventory inventory_ui { get; private set; }
    private bool skill_tree_enabled;
    private bool inventory_enabled;

    private void Awake()
    {
        item_tool_tip = GetComponentInChildren<UI_ItemToolTip>();
        stat_tool_tip = GetComponentInChildren<UI_StatToolTip>();
        skill_tool_tip = GetComponentInChildren<UI_SkillToolTip>();


        skill_tree_ui = GetComponentInChildren<UI_SkillTree>(true); // when you give true to the function it works even when the game object is disabled
        inventory_ui = GetComponentInChildren<UI_Inventory>(true);

        skill_tree_enabled = skill_tree_ui.gameObject.activeSelf;
        inventory_enabled = inventory_ui.gameObject.activeSelf;
    }

    public void ToggleSkillTreeUI()
    {
        skill_tree_enabled = !skill_tree_enabled;
        skill_tree_ui.gameObject.SetActive(skill_tree_enabled);
        skill_tool_tip.ShowToolTip(false, null);
    }

    public void ToggleInventoryUI()
    {
        inventory_enabled = !inventory_enabled;
        inventory_ui.gameObject.SetActive(inventory_enabled);
        stat_tool_tip.ShowToolTip(false, null);
        item_tool_tip.ShowToolTip(false, null);
        
    }
}
