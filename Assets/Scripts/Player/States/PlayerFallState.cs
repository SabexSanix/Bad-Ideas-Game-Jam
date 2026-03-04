using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateManager.PlayerState key, PlayerStateManager stateManager) : base(key, stateManager)
    {
    }
    public override void EnterState()
    {
    }
    public override void UpdateState()
    {
        Flip();
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

        if (stateManager.IsGrounded) return UserInput.MoveVector.x == 0f ? PlayerStateManager.PlayerState.Idle : PlayerStateManager.PlayerState.Run;

        if(stateManager.LastGrounded > 0f)
        {
            return UserInput.MoveVector.x == 0 ? PlayerStateManager.PlayerState.Idle : PlayerStateManager.PlayerState.Run;
        }
        if(stateManager.LastJumpPressed > 0f && stateManager.RemainingJumps > 0)
        {
            return PlayerStateManager.PlayerState.DoubleJump;
        }

        return StateKey;
    }
}
