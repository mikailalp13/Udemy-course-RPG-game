using UnityEngine;

public class SkillObject_Health : Entity_Health
{
    protected override void Die()
    {
        SkillObject_TimeEcho time_echo = GetComponent<SkillObject_TimeEcho>();
        time_echo.HandleDeath();
    }
}
