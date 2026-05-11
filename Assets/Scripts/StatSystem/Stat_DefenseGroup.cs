using System;
using UnityEngine;

[Serializable]
public class Stat_DefenseGroup
{
    // physical defense
    public Stat Armor;
    public Stat Evasion;

    // elemental resistance
    public Stat FireRes;
    public Stat IceRes;
    public Stat LightningRes;
}
