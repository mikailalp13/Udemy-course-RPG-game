using UnityEngine;

public class Enemy_ReaperSpell : MonoBehaviour
{
    private Entity_Combat combat;
    private DamageScaleData damage_scale_data;

    [SerializeField] private LayerMask what_is_target;
    [SerializeField] private Collider2D col;



    public void SetupSpell(Entity_Combat combat, DamageScaleData damage_scale_data)
    {
        this.damage_scale_data = damage_scale_data;
        this.combat = combat;

        Destroy(gameObject, 2f);
    }


    private void EnableCollider() => col.enabled = true;

    private void DisableCollider() => col.enabled = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if collided object is on a layer we want to damage
        if (((1 << collision.gameObject.layer) & what_is_target) != 0)
        {
            combat.PerformAttackOnTarget(collision.transform, damage_scale_data);
            DisableCollider();
        }
    }
}
