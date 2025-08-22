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

    // Define abstract methods that derived states must implement
    public virtual void Enter() {
    }
    public virtual void Exit() {
    }
    public virtual void HandleInput() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && player.CanDash)
            stateMachine.ChangeState(player.DashState);
    }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() { }
}
