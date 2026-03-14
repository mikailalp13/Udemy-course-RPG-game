using UnityEngine;

public class SkillObject_AnimationTriggers : MonoBehaviour
{
    private SkillObject_TimeEcho time_echo;

    private void Awake()
    {
        time_echo = GetComponentInParent<SkillObject_TimeEcho>();
    }

    private void AttackTrigger()
    {
        time_echo.PerformAttack();
    }

    private void TryTerminate(int current_attack_index)
    {
        if (current_attack_index == time_echo.max_attacks)
            time_echo.HandleDeath();
        
    }
}
