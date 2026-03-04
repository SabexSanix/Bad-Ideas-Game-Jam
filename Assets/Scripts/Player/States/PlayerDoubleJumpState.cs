using UnityEngine;

public class PlayerDoubleJumpState : PlayerBaseState
{
    public PlayerDoubleJumpState(PlayerStateManager.PlayerState key, PlayerStateManager stateManager) : base(key, stateManager)
    {
    }

    public override void EnterState()
    {
        Jump(true);
    }
    public override void UpdateState()
    {
        Flip();
        CheckJumpCut();
    }
    public override void FixedUpdateState()
    {
        Move(stateManager.PlayerData.Speed, stateManager.PlayerData.Acceleration, stateManager.PlayerData.Deceleration);
    }
    public override void ExitState()
    {
        stateManager.RB.gravityScale = stateManager.PlayerData.GravityScale; //Reset gravity on exit
    }

    public override PlayerStateManager.PlayerState GetNextState()
    {
        if (UserInput.WasLeapHoldPressed && stateManager.CanLeap)
        {
            return PlayerStateManager.PlayerState.Leap;
        }
        
        if (stateManager.RB.linearVelocityY <= 0f)
        {
            return PlayerStateManager.PlayerState.Fall;
        }
        if (stateManager.IsGrounded) return UserInput.MoveVector.x == 0f ? PlayerStateManager.PlayerState.Idle : PlayerStateManager.PlayerState.Run; //Might Remove (Edge Case)

        return StateKey;
    }
}
