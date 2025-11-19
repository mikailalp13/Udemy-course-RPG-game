using UnityEngine;

public class Object_Chest : MonoBehaviour , IDamageable
{
    private Rigidbody2D rb =>  GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx =>  GetComponent<Entity_VFX>();

    [Header("Open Details")]
    [SerializeField] private Vector2 knockback;
    public bool TakeDamage(float damage, float elemental_damage, ElementType element, Transform damage_dealer)
    {
        fx.PlayeOnDamageVfx();
        anim.SetBool("chestOpen", true);
        rb.linearVelocity = knockback;

        rb.angularVelocity = Random.Range(-200f, 200f);

        return true;
    }
}
