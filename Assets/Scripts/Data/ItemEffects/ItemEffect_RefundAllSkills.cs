using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Item Effect / Refund All Skills", fileName = "Item effect data - Refund All Skills")]

public class ItemEffect_RefundAllSkills : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skill_tree_ui.RefundAllSkills();
    }
}
