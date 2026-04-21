using System.Linq;
using TMPro;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour, ISaveable
{
    [SerializeField] private int skill_points;
    [SerializeField] private TextMeshProUGUI skill_points_text;
    [SerializeField] private UI_TreeConnectHandler[] parent_nodes;

    private UI_TreeNode[] all_tree_nodes;
    public Player_SkillManager skill_manager { get; private set; }


    private void Start()
    {
        UpdateAllConnections();
        UpdateSkillPointsUI();
    }

    private void UpdateSkillPointsUI()
    {
        skill_points_text.text = skill_points.ToString();
        
    }

    public void UnlockDefaultSkills()
    {
        all_tree_nodes = GetComponentsInChildren<UI_TreeNode>(true);
        skill_manager = FindAnyObjectByType<Player_SkillManager>();

        foreach (var node in all_tree_nodes)
            node.UnlockDefaultSkill();
    }

    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skill_nodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skill_nodes)
            node.Refund();
    }
    public bool EnoughSkillPoints(int cost) => skill_points >= cost;
    public void RemoveSkillPoints(int cost)
    {
        skill_points = skill_points - cost;
        UpdateSkillPointsUI();
    } 
    public void AddSkillPoints(int points)
    {
        skill_points = skill_points + points;
        UpdateSkillPointsUI();
    } 


    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parent_nodes)
            node.UpdateAllConnections();
    }

    public void LoadData(GameData data)
    {
        skill_points = data.skill_points;

        foreach (var node in all_tree_nodes)
        {
            string skill_name = node.skill_data.display_name;

            if (data.skill_tree_ui.TryGetValue(skill_name, out bool unlocked) && unlocked)
                node.UnlockWithSavedData();
        }

        foreach (var skill in skill_manager.all_skills)
        {
            if (data.skill_upgrades.TryGetValue(skill.GetSkillType(), out SkillUpgradeType upgrade_type))
            {
                var upgrade_node = all_tree_nodes.FirstOrDefault(node => node.skill_data.upgrade_data.upgrade_type == upgrade_type);

                if (upgrade_node != null)
                    skill.SetSkillUpgrade(upgrade_node.skill_data);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.skill_points = skill_points;
        data.skill_tree_ui.Clear();
        data.skill_upgrades.Clear();

        foreach (var node in all_tree_nodes)
        {
            string skill_name = node.skill_data.display_name;
            data.skill_tree_ui[skill_name] = node.is_unlocked;
        }

        foreach (var skill in skill_manager.all_skills)
        {
            data.skill_upgrades[skill.GetSkillType()] = skill.GetUpgrade();
        }
    }
}
