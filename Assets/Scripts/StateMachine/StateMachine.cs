using UnityEngine;

public class StateMachine
{
    public EntityState currentState { get; private set; }
    private bool can_change_state;
    public void Initialize(EntityState startState)
    {
        can_change_state = true;
        currentState = startState;
        currentState.Enter();
    }
    public void ChangeState(EntityState newState)
    {
        if (can_change_state == false)
            return;
        
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }

    public void SwitchOffStateMachine() => can_change_state = false;
}
