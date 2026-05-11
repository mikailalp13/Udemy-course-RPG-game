using UnityEngine;

public class Entity_AnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entity_combat;


    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entity_combat = GetComponentInParent<Entity_Combat>();
    }


    private void CurrentStateTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }


    private void AttackTrigger()
    {
        entity_combat.PerformAttack();
    }
}
