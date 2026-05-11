using UnityEngine;
using System.Collections;

public class Enemy_Mage : Enemy, ICounterable
{
    public bool CanBeCountered { get => can_be_stunned; }
    public Enemy_MageRetreatState mage_retreat_state { get; private set; }
    public Enemy_MageBattleState mage_battle_state { get; private set; }
    public Enemy_MageSpellCastState mage_spell_cast_state { get; private set; }

    
    [Header("Mage Specifics")]
    [SerializeField] private GameObject spell_prefab;
    [SerializeField] private Transform spell_start_position;
    [SerializeField] private int amount_to_cast = 3;
    [SerializeField] private float spell_cast_cooldown = 0.3f;
    public bool spell_cast_performed { get; private set; }


    [Space]
    public float retreat_cooldown = 5f;
    public float retreat_max_distance = 10f;
    public float retreat_speed = 12f;


    [SerializeField] private Transform behind_collision_check;
    [SerializeField] private bool has_recovery_animation = true;



    protected override void Awake()
    {
        base.Awake();

        idle_state = new Enemy_IdleState(this, state_machine, "idle");
        move_state = new Enemy_MoveState(this, state_machine, "move");
        attack_state = new Enemy_AttackState(this, state_machine, "attack");
        dead_state = new Enemy_DeadState(this, state_machine, "idle");
        stunned_state = new Enemy_StunnedState(this, state_machine, "stunned");

        mage_spell_cast_state = new Enemy_MageSpellCastState(this,state_machine, "spellCast");
        mage_retreat_state = new Enemy_MageRetreatState(this, state_machine, "battle");
        mage_battle_state = new Enemy_MageBattleState(this, state_machine, "battle");
        battle_state = mage_battle_state;

        anim.SetBool("hasStunRecovery", has_recovery_animation);
    }


    protected override void Start()
    {
        base.Start();

        state_machine.Initialize(idle_state);
    }


    public void SetSpellCastPerformed(bool performed) => spell_cast_performed = performed;


    public override void SpecialAttack()
    {
        StartCoroutine(CastSpellCo());
    }


    private IEnumerator CastSpellCo()
    {
        for (int i = 0; i < amount_to_cast; i++)
        {
            Enemy_MageProjectile projectile 
                = Instantiate(spell_prefab, spell_start_position.position, Quaternion.identity).GetComponent<Enemy_MageProjectile>();

            projectile.SetupProjectile(player.transform, combat);
            yield return new WaitForSeconds(spell_cast_cooldown);
        }

        SetSpellCastPerformed(true);
    }


    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        state_machine.ChangeState(stunned_state);
    }


    public bool CantMoveBackwards()
    {
        bool detected_wall = Physics2D.Raycast(behind_collision_check.position, Vector2.right * -facing_dir, 1.5f, what_is_ground);
        bool no_ground = Physics2D.Raycast(behind_collision_check.position, Vector2.down, 1.5f, what_is_ground) == false;

        return no_ground || detected_wall;
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(behind_collision_check.position, 
            new Vector3(behind_collision_check.position.x + (1.5f * -facing_dir), behind_collision_check.position.y));

        Gizmos.DrawLine(behind_collision_check.position, 
            new Vector3(behind_collision_check.position.x, behind_collision_check.position.y - 1.5f));
    }
}
