using UnityEngine;

public class Enemy_ArcherElf : Enemy
{
    public bool CanBeCountered { get => can_be_stunned; }
    public Enemy_ArcherElfBattleState elf_battle_state { get; private set; }
    

    [Header("Archer Elf Specifics")]
    [SerializeField] private GameObject arrow_prefab;
    [SerializeField] private Transform arrow_start_point;
    [SerializeField] private float arrow_speed = 8f;



    protected override void Awake()
    {
        base.Awake();

        idle_state = new Enemy_IdleState(this, state_machine, "idle");
        move_state = new Enemy_MoveState(this, state_machine, "move");
        attack_state = new Enemy_AttackState(this, state_machine, "attack");
        dead_state = new Enemy_DeadState(this, state_machine, "idle");
        stunned_state = new Enemy_StunnedState(this, state_machine, "stunned");

        elf_battle_state = new Enemy_ArcherElfBattleState(this, state_machine, "battle");
        battle_state = elf_battle_state;
    }


    protected override void Start()
    {
        base.Start();

        state_machine.Initialize(idle_state);
    }


    public override void SpecialAttack()
    {
        GameObject new_arrow = Instantiate(arrow_prefab, arrow_start_point.position, Quaternion.identity);
        new_arrow.GetComponent<Enemy_ArcherElfArrow>().SetupArrow(arrow_speed * facing_dir, combat);
    }


    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        state_machine.ChangeState(stunned_state);
    }
}
