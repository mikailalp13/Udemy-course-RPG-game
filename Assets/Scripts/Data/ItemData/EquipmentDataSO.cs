using UnityEngine;
using System;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Equipment Item", fileName = "Equipment data - ")]
public class EquipmentDataSO : ItemDataSO
{
    [Header("Item Modifiers")]
    public ItemModifier[] modifiers;
}

[Serializable]
public class ItemModifier
{
    public StatType stat_type;
    public float value;
}
