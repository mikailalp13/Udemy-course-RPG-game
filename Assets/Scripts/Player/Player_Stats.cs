using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Stats : Entity_Stats
{
    private List<string> active_buff = new List<string>();
    private Inventory_Player inventory;


    protected override void Awake()
    {
        base.Awake();

        inventory = GetComponent<Inventory_Player>();
    }

    public bool CanApplyBuffOf(string source)
    {
        return active_buff.Contains(source) == false; // this way player wont be able to use more than one same buff at the time
    }

    public void ApplyBuff(BuffEffectData[] buffs_to_apply, float duration, string source)
    {
        StartCoroutine(BuffCo(buffs_to_apply, duration, source));
    }

    private IEnumerator BuffCo(BuffEffectData[] buffs_to_apply, float duration, string source)
    {
        active_buff.Add(source);

        foreach (var buff in buffs_to_apply)
        {
            GetStatByType(buff.type).AddModifier(buff.value, source);
        }

        yield return new WaitForSeconds(duration);

        foreach (var buff in buffs_to_apply)
        {
            GetStatByType(buff.type).RemoveModifier(source);
        }

        inventory.TriggerUpdateUI();
        active_buff.Remove(source);
    }
}
