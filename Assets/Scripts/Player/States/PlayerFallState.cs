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
        Move(stateManager.PlayerData.WalkData.Speed, stateManager.PlayerData.WalkData.Acceleration, stateManager.PlayerData.WalkData.Deceleration);
    }
    public override void ExitState()
    {
        stateManager.RB.gravityScale = stateManager.PlayerData.GravityScale; //Reset gravity on exit
    }
    public override PlayerStateManager.PlayerState GetNextState()
    {
        if (stateManager.IsGrounded)
        {
            if (UserInput.WasLeapHoldReleased && stateManager.CanLeap)
            {
                return PlayerStateManager.PlayerState.Leap;
            }
            return UserInput.MoveVector.x == 0f ? PlayerStateManager.PlayerState.Idle : PlayerStateManager.PlayerState.Walk;
        } 

        if(stateManager.LastGrounded > 0f)
        {
            if (UserInput.WasLeapHoldReleased && stateManager.CanLeap)
            {
                return PlayerStateManager.PlayerState.Leap;
            }
            return UserInput.MoveVector.x == 0 ? PlayerStateManager.PlayerState.Idle : PlayerStateManager.PlayerState.Walk;
        }
        if(stateManager.LastJumpPressed > 0f && stateManager.RemainingJumps > 0)
        {
            return PlayerStateManager.PlayerState.DoubleJump;
        }

        return StateKey;
    }
}
