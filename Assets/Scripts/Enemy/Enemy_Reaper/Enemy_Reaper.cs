using UnityEngine;
using System.Collections;

public class Enemy_Reaper : Enemy, ICounterable
{
    public bool CanBeCountered { get => can_be_stunned; }
    public Enemy_ReaperAttackState reaper_attack_state { get; private set; }
    public Enemy_ReaperBattleState reaper_battle_state { get; private set; }
    public Enemy_ReaperTeleportState reaper_teleport_state { get; private set; }
    public Enemy_ReaperSpellCastState reaper_spell_cast_state { get; private set; }


    [Header("Reaper Specifics")]
    public float max_battle_idle_time = 5f;



    [Header("Reaper Spell Cast")]
    [SerializeField] private DamageScaleData spell_damage_scale;
    [SerializeField] private GameObject spell_cast_prefab;
    [SerializeField] private int amount_to_cast = 6;
    [SerializeField] private float spell_cast_rate = 1.5f;
    [SerializeField] private float spell_cast_state_cooldown = 10f;
    [SerializeField] private Vector2 player_offset_prediction;
    private float last_time_casted_spells = float.NegativeInfinity;
    public bool spell_cast_performed { get; private set; }
    private Player player_script;



    [Header("Reaper Teleport")] 
    [SerializeField] private BoxCollider2D arena_bounds;
    [SerializeField] private float offset_center_y = 1.959f;
    [SerializeField] private float chance_to_teleport = 0.25f;
    private float default_teleport_chance;
    public bool teleport_trigger { get; private set; }



    protected override void Awake()
    {
        base.Awake();

        idle_state = new Enemy_IdleState(this, state_machine, "idle");
        move_state = new Enemy_MoveState(this, state_machine, "move");
        dead_state = new Enemy_DeadState(this, state_machine, "idle");
        stunned_state = new Enemy_StunnedState(this, state_machine, "stunned");

        reaper_battle_state = new Enemy_ReaperBattleState(this, state_machine, "battle");
        reaper_attack_state = new Enemy_ReaperAttackState(this, state_machine, "attack");
        reaper_teleport_state = new Enemy_ReaperTeleportState(this, state_machine, "teleport");
        reaper_spell_cast_state = new Enemy_ReaperSpellCastState(this, state_machine, "spellCast");
        
        battle_state = reaper_battle_state;
    }


    protected override void Start()
    {
        base.Start();

        arena_bounds.transform.parent = null;
        default_teleport_chance = chance_to_teleport;
        
        state_machine.Initialize(idle_state);
    }


    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        state_machine.ChangeState(stunned_state);
    }


    public override void SpecialAttack()
    {
        StartCoroutine(CastSpellCo());
    }


    private IEnumerator CastSpellCo()
    {
        if (player_script == null)
            player_script = player.GetComponent<Player>();

        for (int i = 0; i < amount_to_cast; i++)
        {
            bool player_moving = player_script.rb.linearVelocity.magnitude > 0;
            float x_offset = player_moving ? player_offset_prediction.x * player_script.facing_dir : 0;
            Vector3 spell_position = player.transform.position + new Vector3(x_offset, player_offset_prediction.y);


            Enemy_ReaperSpell spell 
                    = Instantiate(spell_cast_prefab, spell_position, Quaternion.identity).GetComponent<Enemy_ReaperSpell>(); 
                
            spell.SetupSpell(combat, spell_damage_scale);

            yield return new WaitForSeconds(spell_cast_rate);
        }


        SetSpellCastPerformed(true);
    }


    public void SetSpellCastPerformed(bool spell_cast_status) => spell_cast_performed = spell_cast_status;

    public bool CanDoSpellCast() => Time.time > last_time_casted_spells + spell_cast_state_cooldown;

    public void SetSpellCastOnCooldown() => last_time_casted_spells = Time.time;


    public bool ShouldTeleport()
    {
        if (Random.value < chance_to_teleport)
        {
            chance_to_teleport = default_teleport_chance;
            return true;
        }

        chance_to_teleport += 0.05f;
        return false;
    }


    public void SetTeleportTrigger(bool trigger_status) => teleport_trigger = trigger_status;


    public Vector3 FindTeleportPoint()
    {
        int max_attempts = 10;
        float reaper_with_collider_half = col.bounds.size.x / 2;
        
        for (int i = 0; i < max_attempts; i++)
        {
            float random_x = Random.Range(arena_bounds.bounds.min.x + reaper_with_collider_half, 
                arena_bounds.bounds.max.x - reaper_with_collider_half);

            Vector2 raycast_point = new Vector2(random_x, arena_bounds.bounds.max.y);

            RaycastHit2D hit = Physics2D.Raycast(raycast_point, Vector2.down, Mathf.Infinity, what_is_ground);

            if (hit.collider != null)
                return hit.point + new Vector2(0, offset_center_y);
        }

        return transform.position;
    }
}
