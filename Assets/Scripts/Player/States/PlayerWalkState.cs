using System.Collections;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateManager.PlayerState key, PlayerStateManager stateManager) : base(key, stateManager)
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

    public override PlayerStateManager.PlayerState GetNextState()
    {
        if (UserInput.WasLeapHoldPressed && stateManager.CanLeap)
        {
            return PlayerStateManager.PlayerState.Leap;
        }

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
            return UserInput.MoveVector.x == 0f ? PlayerStateManager.PlayerState.Idle : PlayerStateManager.PlayerState.Run;
        }
    }
}
