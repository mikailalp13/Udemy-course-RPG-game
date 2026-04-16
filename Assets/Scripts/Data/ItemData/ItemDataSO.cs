using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Material Item", fileName = "Material data - ")]
public class ItemDataSO : ScriptableObject
{
    [Header("Merchant Details")]
    [Range(0, 10000)]
    public int item_price = 100;
    public int min_stack_size_at_shop = 1;
    public int max_stack_size_at_shop = 1;
    

    [Header("Drop Details")]
    [Range(0, 1000)]
    public int item_rarity = 100;

    [Range(0, 100)]
    public float drop_chance;

    [Range(0, 100)]
    public float max_drop_chance = 65f;



    [Header("Craft Details")]
    public Inventory_Item[] craft_recipe;


    [Header("Item Details")]
    public string item_name;
    public Sprite item_icon;
    public ItemType item_type;
    public int max_stack_size = 1;

    [Header("Item Effect")]
    public ItemEffectDataSO item_effect;


    private void OnValidate()
    {
        drop_chance = GetDropChance();
    }

    public float GetDropChance()
    {
        float max_rarity = 1000;
        float chance = (max_rarity - item_rarity + 1) / max_rarity * 100;

        return Mathf.Min(chance, max_drop_chance);
    }
}
