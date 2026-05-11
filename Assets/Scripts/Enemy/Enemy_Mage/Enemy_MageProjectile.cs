using UnityEngine;

public class Enemy_MageProjectile : MonoBehaviour
{
    private Entity_Combat combat;
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;

    [SerializeField] private float arc_height = 2f;
    [SerializeField] private LayerMask what_can_collide_with;



    public void SetupProjectile(Transform target, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        anim.enabled = false;
        this.combat = combat;

        Vector2 velocity = CalculateBallisticVelocity(transform.position, target.position);
        rb.linearVelocity = velocity;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & what_can_collide_with) != 0)
        {
            combat.PerformAttackOnTarget(collision.transform);
            
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            anim.enabled = true;
            col.enabled = false;
            Destroy(gameObject, 2);
        }
    }


    private Vector2 CalculateBallisticVelocity(Vector2 start, Vector2 end)
    {
        // get effective gravity based on global gravity and this rigidbody's gravity scale
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        // calculate vertical and horizontal displacement
        float displacement_x = end.x - start.x;
        float displacement_y = end.y - start.y;

        // ensure arc is always above
        float peak_hight = Mathf.Max(arc_height, end.y - start.y + 0.1f); 

        // time to reach the top of the arc
        float time_to_apex = Mathf.Sqrt(2 * peak_hight / gravity);
        // time to fall from the top of arc to the target
        float time_from_apex = Mathf.Sqrt(2 * (peak_hight - displacement_y) / gravity);

        // total flight time = up + down
        float total_time = time_to_apex + time_from_apex;

        // initial horizontal velocity to cover distance in total flight time
        float velocity_x = displacement_x / total_time;
        // initial vertical velocity to reach the arc height
        float velocity_y = Mathf.Sqrt(2 * gravity * peak_hight);
        
        // return combined velocity
        return new Vector2(velocity_x, velocity_y);
    }
}
