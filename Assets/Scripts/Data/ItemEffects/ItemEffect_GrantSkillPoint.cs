using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Item Effect / Grant Skill Point", fileName = "Item effect data - Grant Skill Point")]

public class ItemEffect_GrantSkillPoint : ItemEffectDataSO
{
    [SerializeField] private int points_to_add;

    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skill_tree_ui.AddSkillPoints(points_to_add);
    }
}
