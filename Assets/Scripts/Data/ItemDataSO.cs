using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Material Item", fileName = "Material data - ")]
public class ItemDataSO : ScriptableObject
{
    public string item_name;
    public Sprite item_icon;
    public ItemType item_type;
    public int max_stack_size = 1;

    [Header("Item Effect")]
    public ItemEffectDataSO item_effect;

    
}
