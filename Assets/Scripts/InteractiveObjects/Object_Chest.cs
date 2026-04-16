using UnityEngine;

public class Object_Chest : MonoBehaviour , IDamageable
{
    private Rigidbody2D rb =>  GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx =>  GetComponent<Entity_VFX>();
    private Entity_DropManager drop_manager => GetComponent<Entity_DropManager>();


    [Header("Open Details")]
    [SerializeField] private Vector2 knockback;
    [SerializeField] private bool can_drop_items = true;

    public bool TakeDamage(float damage, float elemental_damage, ElementType element, Transform damage_dealer)
    {
        if (can_drop_items == false)
            return false;

        can_drop_items = false;
        drop_manager?.DropItems();
        
        fx.PlayeOnDamageVfx();

        anim.SetBool("chestOpen", true);

        rb.linearVelocity = knockback;
        rb.angularVelocity = Random.Range(-200f, 200f);

        return true;
    }
}
