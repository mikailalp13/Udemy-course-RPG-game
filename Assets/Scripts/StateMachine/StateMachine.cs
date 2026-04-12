using UnityEngine;

public class StateMachine
{
    public EntityState current_state { get; private set; }
    private bool can_change_state;
    public void Initialize(EntityState start_state)
    {
        can_change_state = true;
        current_state = start_state;
        current_state.Enter();
    }
    public void ChangeState(EntityState new_state)
    {
        if (can_change_state == false)
            return;
        
        current_state.Exit();
        current_state = new_state;
        current_state.Enter();
    }

    public void UpdateActiveState()
    {
        current_state.Update();
    }

    public void SwitchOffStateMachine() => can_change_state = false;
}
