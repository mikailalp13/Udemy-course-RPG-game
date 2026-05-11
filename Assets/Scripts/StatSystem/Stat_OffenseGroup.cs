using System;
using UnityEngine;

[Serializable]
public class Stat_OffenseGroup
{
    public Stat AttackSpeed;
    // physical damage
    public Stat Damage;
    public Stat CritPower;
    public Stat CritChance;
    public Stat ArmorReduction;

    
    // elemental damage 

    public Stat FireDamage;
    public Stat IceDamage;
    public Stat LightningDamage;
}
