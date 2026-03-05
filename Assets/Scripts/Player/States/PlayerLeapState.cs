using System.Collections;
using UnityEngine;

public class PlayerLeapState : PlayerBaseState
{
    public PlayerLeapState(PlayerStateManager.PlayerState key, PlayerStateManager stateManager) : base(key, stateManager)
    {
    }

    public override void EnterState()
    {
        stateManager.StartCoroutine(IDash());
        
    }
    private IEnumerator IDash()
    {
        stateManager.CanLeap = false;
        stateManager.IsLeaping = true;

        // float gravityScale = stateManager.RB.gravityScale;
        // stateManager.RB.gravityScale = 0.0f; 
        Vector2 velocity = new Vector2(stateManager.PlayerData.LeapForce.x * stateManager.transform.right.x, stateManager.PlayerData.LeapForce.y);
        stateManager.RB.linearVelocity = velocity;

        yield return new WaitForSeconds(stateManager.PlayerData.LeapTime); 

        // stateManager.RB.gravityScale = gravityScale;
        stateManager.IsLeaping = false;
        yield return new WaitForSeconds(stateManager.PlayerData.DashCoolDown); //Reset CanLeap after cooldown
        stateManager.CanLeap = true;
    }
    public override PlayerStateManager.PlayerState GetNextState()
    {
        if (stateManager.IsLeaping) return StateKey;

        if (stateManager.LastGrounded > 0f)
        {
            if (stateManager.LastJumpPressed > 0f)
            {
                return PlayerStateManager.PlayerState.Jump;
            }
            else
            {
                return UserInput.MoveVector.x == 0f ? PlayerStateManager.PlayerState.Idle : PlayerStateManager.PlayerState.Walk;
            }
        }
        else
        {
            return PlayerStateManager.PlayerState.Fall;
        }
    }
}
