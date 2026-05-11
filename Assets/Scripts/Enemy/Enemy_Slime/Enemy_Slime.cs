using UnityEngine;

public class Enemy_Slime : Enemy, ICounterable
{
    public bool CanBeCountered { get => can_be_stunned; }
    public Enemy_SlimeDeadState slime_dead_state { get; private set; }
    

    [Header("Slime Specifics")]
    [SerializeField] private GameObject slime_to_create_prefab;
    [SerializeField] private int amount_of_slimes_to_create = 2;
    [SerializeField] private bool has_recovery_animation = true;



    protected override void Awake()
    {
        base.Awake();


        idle_state = new Enemy_IdleState(this, state_machine, "idle");
        move_state = new Enemy_MoveState(this, state_machine, "move");
        attack_state = new Enemy_AttackState(this, state_machine, "attack");
        battle_state = new Enemy_BattleState(this, state_machine, "battle");
        stunned_state = new Enemy_StunnedState(this, state_machine, "stunned");

        slime_dead_state = new Enemy_SlimeDeadState(this, state_machine, "idle");

        anim.SetBool("hasStunRecovery", has_recovery_animation);
    }


    protected override void Start()
    {
        base.Start();

        state_machine.Initialize(idle_state);
    }


    public override void EntityDeath()
    {
        state_machine.ChangeState(slime_dead_state);
    }


    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        state_machine.ChangeState(stunned_state);
    }


    public void CreateSlimeOnDeath()
    {
        if (slime_to_create_prefab == null)
            return;

        for (int i = 0; i < amount_of_slimes_to_create; i++)
        {
            GameObject new_slime = Instantiate(slime_to_create_prefab, transform.position, Quaternion.identity);
            Enemy_Slime slime_script = new_slime.GetComponent<Enemy_Slime>();

            slime_script.stats.AdjustStatSetup(stats.resources, stats.offense, stats.defense, 0.6f, 1.2f);
            slime_script.ApplyRespawnVelocity();
            slime_script.StartBattleStateCheck(player);
        }
    }


    public void ApplyRespawnVelocity()
    {
        Vector2 velocity = new Vector2(stunned_velocity.x * Random.Range(-1f, 1f), stunned_velocity.y * Random.Range(1f, 2f));
        SetVelocity(velocity.x, velocity.y);
    }


    public void StartBattleStateCheck(Transform player)
    {
        TryEnterBattleState(player);
        InvokeRepeating(nameof(ReEnterBattleState), 0, 0.3f);
    }


    public void ReEnterBattleState()
    {
        if (state_machine.current_state == battle_state || state_machine.current_state == attack_state)
        {
            CancelInvoke(nameof(ReEnterBattleState));
            return;
        }
        state_machine.ChangeState(battle_state);
    }
}
