using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skill_tool_tip;
    public UI_SkillTree skill_tree;
    private bool skill_tree_enabled;

    private void Awake()
    {
        skill_tool_tip = GetComponentInChildren<UI_SkillToolTip>();
        skill_tree = GetComponentInChildren<UI_SkillTree>(true); // when you give true to the function it works even when the game object is disabled
    }

    public void ToggleSkillTreeUI()
    {
        skill_tree_enabled = !skill_tree_enabled;
        skill_tree.gameObject.SetActive(skill_tree_enabled);
        skill_tool_tip.ShowToolTip(false, null);
    }
}
