using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private Skill_DomainExpansion domain_manager;

    private float expand_speed = 2f;
    private float duration;

    private float slow_down_percent = 0.9f;

    private Vector3 target_scale;
    private bool is_shrinking;


    public void SetupDomain(Skill_DomainExpansion domain_manager)
    {
        this.domain_manager = domain_manager;

        duration = domain_manager.GetDomainDuration();
        slow_down_percent = domain_manager.GetSlowPercentage();
        expand_speed = domain_manager.expand_speed;
        float max_size = domain_manager.max_domain_size;

        target_scale = Vector3.one * max_size;
        Invoke(nameof(ShrinkDomain), duration);
    }

    private void Update()
    {
        HandleScaling();
    }

    private void HandleScaling()
    {
        float size_difference = Mathf.Abs(transform.localScale.x - target_scale.x);
        bool should_change_scale = size_difference > 0.1f;

        if (should_change_scale)
            transform.localScale = Vector3.Lerp(transform.localScale, target_scale, expand_speed * Time.deltaTime);

        if (is_shrinking && size_difference < 0.1f)
            TerminateDomain();
    }

    private void TerminateDomain()
    {
        domain_manager.ClearTargets();
        Destroy(gameObject);
    }

    private void ShrinkDomain()
    {
        target_scale = Vector3.zero;
        is_shrinking = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
            return;
        
        domain_manager.AddTarget(enemy);
        enemy.SlowDownEntity(duration, slow_down_percent, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        
        if (enemy == null)
            return;

        enemy.StopSlowDown();
    }
}
