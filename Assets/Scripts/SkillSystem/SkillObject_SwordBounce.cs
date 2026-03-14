using UnityEngine;
using System.Collections.Generic;

public class SkillObject_SwordBounce : SkillObject_Sword
{
    [SerializeField] private float bounce_speed = 15f;
    private int bounce_count;
    private Collider2D[] enemy_targets;
    private Transform next_target;
    private List<Transform> selected_before = new List<Transform>();


    public override void SetupSword(Skill_SwordThrow sword_manager, Vector2 direction)
    {
        anim.SetTrigger("spin");
        base.SetupSword(sword_manager, direction);

        bounce_speed = sword_manager.bounce_speed;
        bounce_count = sword_manager.bounce_count;
    }
    protected override void Update()
    {
        HandleComeBack();
        HandleBounce();
    }

    private void HandleBounce()
    {
        if (next_target == null)
            return;
        
        transform.position = Vector2.MoveTowards(transform.position, next_target.position, bounce_speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, next_target.position) < 0.75f)
        {
            DamageEnemiesInRadius(transform, 1);
            BounceToNextTarget();

            if (bounce_count == 0 || next_target == null)
            {
                next_target = null;
                GetSwordBackToPlayer();
            }
        }
    }

    private void BounceToNextTarget()
    {
        next_target = GetNextTarget();
        bounce_count--;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemy_targets == null)
        {
            enemy_targets = GetEnemiesAround(transform, 10);
            rb.simulated = false;
        }

        DamageEnemiesInRadius(transform, 1);

        if (enemy_targets.Length <= 1 || bounce_count == 0)
            GetSwordBackToPlayer();
        else
            next_target = GetNextTarget();
    }

    private Transform GetNextTarget()
    {
        List<Transform> valid_target = GetValidTargets();

        int random_index = Random.Range(0, valid_target.Count);

        Transform next_target = valid_target[random_index];
        selected_before.Add(next_target);

        return next_target;
    }

    private List<Transform> GetValidTargets()
    {
        List<Transform> valid_targets = new List<Transform>();
        List<Transform> alive_targets = GetAliveTargets();

        foreach (var enemy in alive_targets)
        {
            if (enemy != null && selected_before.Contains(enemy.transform) == false)
                valid_targets.Add(enemy.transform);
        }

        if (valid_targets.Count > 0)
            return valid_targets;
        else
        {
            selected_before.Clear();
            return alive_targets;
        }
    }
    private List<Transform> GetAliveTargets()
    {
        List<Transform> alive_targets = new List<Transform>();

        foreach (var enemy in enemy_targets)
        {
            if (enemy != null)
                alive_targets.Add(enemy.transform);
        }

        return alive_targets;
    }
}
