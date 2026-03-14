using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    private SkillObject_Sword current_sword;
    private float current_throw_power;


    [Header("Regular Sword Upgrade")]
    [SerializeField] private GameObject sword_prefab;
    [Range(0, 10)]
    [SerializeField] private float regular_throw_power = 5f;


    [Header("Pierce Sword Upgrade")]
    [SerializeField] private GameObject pierce_sword_prefab;
    public int amount_to_pierce = 2;
    [Range(0, 10)]
    [SerializeField] private float pierce_throw_power = 5f;


    [Header("Spin Sword Upgrade")]
    [SerializeField] private GameObject spin_sword_prefab;
    public int max_distance = 5;
    public float attacks_per_second = 6f;
    public float max_spin_duration = 3f;
    [Range(0, 10)]
    [SerializeField] private float spin_throw_power = 5f;


    [Header("Bounce Sword Upgrade")]
    [SerializeField] private GameObject bounce_sword_prefab;
    public int bounce_count = 5;
    public float bounce_speed = 12f;
    [Range(0, 10)]
    [SerializeField] private float bounce_throw_power = 5f;


    [Header("Trajectory Prediction")]
    [SerializeField] private GameObject prediction_dot;
    [SerializeField] private int number_of_dots = 20;
    [SerializeField] private float space_between_dots = 0.05f;
    private float sword_gravity;
    private Transform[] dots;
    private Vector2 confirmed_direction;


    protected override void Awake()
    {
        base.Awake();

        sword_gravity = sword_prefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanUseSkill()
    {
        UpdateThrowPower();

        if (current_sword != null)
        {
            current_sword.GetSwordBackToPlayer();
            return false;
        }

        return base.CanUseSkill();
    }

    public void ThrowSword()
    {
        GameObject sword_prefab = GetSwordPrefab();
        GameObject new_sword = Instantiate(sword_prefab, dots[1].position, Quaternion.identity);

        current_sword = new_sword.GetComponent<SkillObject_Sword>();
        current_sword.SetupSword(this, GetThrowPower());

        SetSkillOnCooldown();
    }

    private GameObject GetSwordPrefab()
    {
        if (Unlocked(SkillUpgradeType.SwordThrow))
            return sword_prefab;
        
        if (Unlocked(SkillUpgradeType.SwordThrow_Pierce))
            return pierce_sword_prefab;

        if (Unlocked(SkillUpgradeType.SwordThrow_Spin))
            return spin_sword_prefab;
        
        if (Unlocked(SkillUpgradeType.SwordThrow_Bounce))
            return bounce_sword_prefab;

        Debug.Log("No valid sword upgrade selected!");
        return null;
    }

    private void UpdateThrowPower()
    {
        switch (upgrade_type)
        {
            case SkillUpgradeType.SwordThrow:
                current_throw_power = regular_throw_power;
                break;
            case SkillUpgradeType.SwordThrow_Pierce:
                current_throw_power = pierce_throw_power;
                break;
            case SkillUpgradeType.SwordThrow_Spin:
                current_throw_power = spin_throw_power;
                break;
            case SkillUpgradeType.SwordThrow_Bounce:
                current_throw_power = bounce_throw_power;
                break;
        }
    }

    private Vector2 GetThrowPower() => confirmed_direction * (current_throw_power * 9);   

    public void PredictTrajectory(Vector2 direction)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].position = GetTrajectoryPoint(direction, i * space_between_dots);
        }
    }

    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaled_throw_power = current_throw_power * 10f;

        // this gives us the initial velocity - the starting speed and direction of the throw
        Vector2 initial_velocity = direction * scaled_throw_power;

        //gravity pulls the sword down over time. The longer it's in the air, the more it drops.
        Vector2 gravity_effect = 0.5f * Physics2D.gravity * sword_gravity * (t * t);

        // We calculate how far the sword will travel after time 't'
        // by combining the initial throw direction with the gravity pull
        Vector2 predicted_point = (initial_velocity * t) + gravity_effect;

        Vector2 player_position = transform.root.position;

        return player_position + predicted_point;
    }

    public void ConfirmTrajectory(Vector2 direction) => confirmed_direction = direction;

    public void EnableDots(bool enable)
    {
        foreach (Transform t in dots)   
            t.gameObject.SetActive(enable);
    }

    private Transform[] GenerateDots()
    {
        Transform[] new_dots = new Transform[number_of_dots];

        for (int i = 0; i < number_of_dots; i++)
        {
            new_dots[i] = Instantiate(prediction_dot, transform.position, Quaternion.identity, transform).transform;
            new_dots[i].gameObject.SetActive(false);
        }
        return new_dots;
    }
}
