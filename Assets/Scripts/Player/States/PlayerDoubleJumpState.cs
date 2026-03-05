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
        Move(stateManager.PlayerData.WalkData.Speed, stateManager.PlayerData.WalkData.Acceleration, stateManager.PlayerData.WalkData.Deceleration);
    }
    public override void ExitState()
    {
        stateManager.RB.gravityScale = stateManager.PlayerData.GravityScale; //Reset gravity on exit
    }

    public override PlayerStateManager.PlayerState GetNextState()
    {
        if (stateManager.RB.linearVelocityY <= 0f)
        {
            return PlayerStateManager.PlayerState.Fall;
        }
        if (stateManager.IsGrounded)
        {
            if (UserInput.WasLeapHoldReleased && stateManager.CanLeap)
            {
                return PlayerStateManager.PlayerState.Leap;
            }
            return UserInput.MoveVector.x == 0f ? PlayerStateManager.PlayerState.Idle : PlayerStateManager.PlayerState.Walk; //Might Remove (Edge Case)
        }

        return StateKey;
    }
}
