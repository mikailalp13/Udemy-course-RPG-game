using UnityEngine;

public class ItemEffectDataSO : ScriptableObject
{
    [TextArea]
    public string effect_description;
    protected Player player;


    public virtual bool CanBeUsed()
    {
        return true;
    }

    public virtual void ExecuteEffect()
    {
        
    }

    public virtual void Subscribe(Player player)
    {
        this.player = player;
    }

    public virtual void Unsubscribe()
    {
        
    }
}
