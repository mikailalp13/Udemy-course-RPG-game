using TMPro;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour
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
}
