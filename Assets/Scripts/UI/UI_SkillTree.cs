using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skill_points;
    [SerializeField] private UI_TreeConnectHandler[] parent_nodes;
    public Player_SkillManager skill_manager { get; private set; }


    private void Awake()
    {
        skill_manager = FindAnyObjectByType<Player_SkillManager>();
    }

    private void Start()
    {
        UpdateAllConnections();
    }

    [ContextMenu("Reset Skill Tree")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skill_nodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skill_nodes)
            node.Refund();
    }
    public bool EnoughSkillPoints(int cost) => skill_points >= cost;
    public void RemoveSkillPoints(int cost) => skill_points = skill_points - cost;
    public void AddSkillPoints(int points) => skill_points = skill_points + points;


    [ContextMenu("Update All Connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parent_nodes)
            node.UpdateAllConnections();
    }
}
