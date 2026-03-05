using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateManager.PlayerState key, PlayerStateManager stateManager) : base(key, stateManager)
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
        Move(stateManager.PlayerData.RunData.Speed, stateManager.PlayerData.RunData.Acceleration, stateManager.PlayerData.RunData.Deceleration);
    }

    public override PlayerStateManager.PlayerState GetNextState()
    {
        if (stateManager.LastJumpPressed > 0f)
        {
            return PlayerStateManager.PlayerState.Jump;
        }
        if (stateManager.LastGrounded <= 0f)
        {
            return PlayerStateManager.PlayerState.Fall;
        }
        else
        {
            if (UserInput.WasLeapHoldReleased && stateManager.CanLeap)
            {
                return PlayerStateManager.PlayerState.Leap;
            }
            return UserInput.MoveVector.x == 0f ? PlayerStateManager.PlayerState.Idle : PlayerStateManager.PlayerState.Walk;
        }
    }
}
