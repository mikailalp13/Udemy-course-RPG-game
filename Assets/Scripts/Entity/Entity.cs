using UnityEngine;
using System;
using System.Collections;

public class Entity : MonoBehaviour
{
    public event Action on_flipped;


    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Entity_Stats stats { get; private set; }
    protected StateMachine state_machine;


    private bool facing_right = true;
    public int facing_dir { get; private set; } = 1;


    [Header("Collision Detection")]
    [SerializeField] protected LayerMask what_is_ground;
    [SerializeField] private float ground_check_distance;
    [SerializeField] private float wall_check_distance;
    [SerializeField] private Transform ground_check;
    [SerializeField] private Transform primary_wall_check;
    [SerializeField] private Transform secondary_wall_check;
    public bool ground_detected { get; private set; }
    public bool wall_detected { get; private set; }

    // Status Variables
    private bool is_knocked;
    private Coroutine knockback_co;
    private Coroutine slow_down_co;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Entity_Stats>();

        state_machine = new StateMachine();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        state_machine.UpdateActiveState();
    }


    public void CurrentStateAnimationTrigger()
    {
        state_machine.currentState.AnimationTrigger();
    }

    public virtual void EntityDeath()
    {

    }


    public virtual void SlowDownEntity(float duration, float slow_multiplier)
    {
        if (slow_down_co != null)
            StopCoroutine(slow_down_co);

        slow_down_co = StartCoroutine(SlowDownEntityCo(duration, slow_multiplier));
    }
    

    protected virtual IEnumerator SlowDownEntityCo(float duration, float slow_multiplier)
    {
        yield return null;
    }


    public void RecieveKnockback(Vector2 knockback, float duration)
    {
        if (knockback_co != null)
            StopCoroutine(knockback_co);
        knockback_co = StartCoroutine(KnockbackCo(knockback, duration));
    }
    private IEnumerator KnockbackCo(Vector2 knocback, float duration)
    {
        is_knocked = true;
        rb.linearVelocity = knocback;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        is_knocked = false;
    }
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (is_knocked)
            return;

        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facing_right == false)
            Flip();
        else if (xVelocity < 0 && facing_right == true)
            Flip();
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facing_right = !facing_right;
        facing_dir = facing_dir * -1;

        on_flipped?.Invoke();
    }

    private void HandleCollisionDetection()
    {
        ground_detected = Physics2D.Raycast(ground_check.position, Vector2.down, ground_check_distance, what_is_ground);

        if (secondary_wall_check != null)
        {
            wall_detected = Physics2D.Raycast(primary_wall_check.position, Vector2.right * facing_dir, wall_check_distance, what_is_ground)
                        && Physics2D.Raycast(secondary_wall_check.position, Vector2.right * facing_dir, wall_check_distance, what_is_ground);
        }
        else
            wall_detected = Physics2D.Raycast(primary_wall_check.position, Vector2.right * facing_dir, wall_check_distance, what_is_ground);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(ground_check.position, ground_check.position + new Vector3(0, -ground_check_distance));
        Gizmos.DrawLine(primary_wall_check.position, primary_wall_check.position + new Vector3(wall_check_distance * facing_dir, 0));

        if (secondary_wall_check != null)
            Gizmos.DrawLine(secondary_wall_check.position, secondary_wall_check.position + new Vector3(wall_check_distance * facing_dir, 0));
    }

}
