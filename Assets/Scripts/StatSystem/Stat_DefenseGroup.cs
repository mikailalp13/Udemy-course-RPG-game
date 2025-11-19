using System;
using UnityEngine;

[Serializable]
public class Stat_DefenseGroup
{
    // physical defense
    public Stat armor;
    public Stat evasion;

    // elemental resistance
    public Stat fire_res;
    public Stat ice_res;
    public Stat lightning_res;
}
