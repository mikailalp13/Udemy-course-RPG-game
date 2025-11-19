using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] float base_value;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>(); // we can modify lists dynamically
    private bool need_to_calculate = true; 
    private float final_value;

    public float GetValue()
    {
        // since we use GetValue a lot, we can make a bool to use it only when somethings changed
        if (need_to_calculate)
        {
            final_value = GetFinalValue();
            need_to_calculate = false;
        }

        return final_value;
    }

    public void AddModifier(float value, string source)
    {
        StatModifier mod_to_add = new StatModifier(value, source);
        modifiers.Add(mod_to_add);
        need_to_calculate = true;
    }

    public void RemoveModifier(string source)
    {
        modifiers.RemoveAll(modifier => modifier.source == source); 
        // just like foreach. we "remove all" if the condition is true. modifier is the name like foreach (var modifier in ...)
        need_to_calculate = true;
    }

    private float GetFinalValue()
    {
        float final_value = base_value;

        foreach (var modifier in modifiers)
        {
            final_value += modifier.value;
        }

        return final_value;
    }

    public float SetBaseValue(float value) => base_value = value;
    
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source; // buff, item etc.

    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
