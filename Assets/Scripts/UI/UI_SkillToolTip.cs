using UnityEngine;
using TMPro;
using System.Text;
using System;
using System.Collections;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI ui;
    private UI_SkillTree skill_tree;

    [SerializeField] private TextMeshProUGUI skill_name;
    [SerializeField] private TextMeshProUGUI skill_description; 
    [SerializeField] private TextMeshProUGUI skill_requirements;

    [Space]
    [SerializeField] private string met_condition_hex = "#76CB18";
    [SerializeField] private string not_met_condition_hex = "#CA1836";
    [SerializeField] private string important_info_hex = "#1AD4D6";
    [SerializeField] private Color example_color;
    [SerializeField] private string locked_skill_text = "You've taken a different path - this skill is now locked.";

    private Coroutine text_effect_co;

    protected override void Awake()
    {
        base.Awake();

        ui = GetComponentInParent<UI>();
        skill_tree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }

    public override void ShowToolTip(bool show, RectTransform target_rect)
    {
        base.ShowToolTip(show, target_rect);
    }

    public void ShowToolTip(bool show, RectTransform target_rect, UI_TreeNode node)
    {
        base.ShowToolTip(show, target_rect);

        if (show == false)
            return;

        skill_name.text = node.skill_data.display_name;
        skill_description.text = node.skill_data.description;

        string skill_locked_text = GetColoredText(important_info_hex, locked_skill_text);
        string requirements = node.is_locked ? skill_locked_text : GetRequirements(node.skill_data.cost, node.needed_nodes, node.conflict_nodes);

        skill_requirements.text = requirements;
    }

    public void LockedSkillEffect()
    {
        if (text_effect_co != null)
            StopCoroutine(text_effect_co);
        
        text_effect_co = StartCoroutine(TextBlinkEffectCo(skill_requirements, 0.15f, 3));
    }

    private IEnumerator TextBlinkEffectCo(TextMeshProUGUI text, float blink_interval, int blink_count)
    {
        for (int i = 0; i < blink_count; i++)
        {
            text.text = GetColoredText(not_met_condition_hex, locked_skill_text);    
            yield return new WaitForSeconds(blink_interval);

            text.text = GetColoredText(important_info_hex, locked_skill_text);
            yield return new WaitForSeconds(blink_interval);
        }
    }

    private string GetRequirements(int skill_cost, UI_TreeNode[] needed_nodes, UI_TreeNode[] conflict_nodes)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Requirements: ");

        string cost_color = skill_tree.EnoughSkillPoints(skill_cost) ? met_condition_hex : not_met_condition_hex;
        string cost_text = $"- {skill_cost} skill point(s)";
        string final_cost_text = GetColoredText(cost_color, cost_text);


        sb.AppendLine(final_cost_text);

        foreach (var node in needed_nodes)
        {
            if (node == null)
                continue;
                
            string node_color = node.is_unlocked ? met_condition_hex : not_met_condition_hex;
            string node_text = $"- {node.skill_data.display_name}";
            string final_node_text = GetColoredText(node_color, node_text);

            sb.AppendLine(final_node_text);
        }

        if (conflict_nodes.Length <= 0)
            return sb.ToString();
        
        sb.AppendLine(); // space
        sb.AppendLine(GetColoredText(important_info_hex, "Locks out: "));

        foreach (var node in conflict_nodes)
        {
            if (node == null)
                continue;

            string node_text = $"- {node.skill_data.display_name}";
            string final_node_text = GetColoredText(important_info_hex, node_text);
            sb.AppendLine(final_node_text);
        }

        return sb.ToString();
    }
}
