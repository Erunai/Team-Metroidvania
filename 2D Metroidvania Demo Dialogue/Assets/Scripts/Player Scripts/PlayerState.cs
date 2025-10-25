using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController player;
    protected PlayerStateManager stateMachine;

    public PlayerState(PlayerController player, PlayerStateManager stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    // Define virtual methods that derived states must implement
    // Note: Abstract methods must be implemented by derived classes, while virtual methods can be optionally overridden
    public abstract void Enter(); // Called when the state is entered
    public virtual void Exit() { } // Called when the state is exited
    public virtual void HandleInput() {
        // I don't know if I want this -- but it might be easier to override the method in states which don't use this
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.CanDash)
            stateMachine.ChangeState(player.DashState);
    }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
