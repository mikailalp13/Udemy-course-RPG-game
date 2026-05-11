using UnityEngine;

public class Enemy_ArcherElfArrow : MonoBehaviour, ICounterable
{
    [SerializeField] private LayerMask what_is_target;

    private Collider2D col;
    private Rigidbody2D rb;
    private Entity_Combat combat;
    private Animator anim;


    public bool CanBeCountered => true;


    public void SetupArrow(float x_velocity, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();

        this.combat = combat;
        rb.linearVelocity = new Vector2(x_velocity, 0);

        if (rb.linearVelocity.x < 0)
            transform.Rotate(0, 180, 0);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // check if collided object is on a layer we want to damage
        if (((1 << collision.gameObject.layer) & what_is_target) != 0)
        {
            combat.PerformAttackOnTarget(collision.transform);
            StuckIntoTarget(collision.transform);
        }
    }


    private void StuckIntoTarget(Transform target)
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;
        anim.enabled = false;

        transform.parent = target;

        Destroy(gameObject, 3);
    }


    public void HandleCounter()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * -1, 0);
        transform.Rotate(0, 180, 0);

        int enemy_layer = LayerMask.NameToLayer("Enemy");

        what_is_target = what_is_target | (1 << enemy_layer);
    }
}
