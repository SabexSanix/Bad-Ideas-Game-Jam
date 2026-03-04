using UnityEngine;

public class PlayerBaseState : BaseState<PlayerStateManager.PlayerState>
{
    public PlayerStateManager stateManager;

    public PlayerBaseState(PlayerStateManager.PlayerState key, PlayerStateManager stateManager) : base(key)
    {
        this.stateManager = stateManager;
    }

    public override void EnterState()
    {

    }
    public override void UpdateState()
    {
    }
    public override void FixedUpdateState()
    {
    }
    public override void ExitState()
    {

    }

    public override PlayerStateManager.PlayerState GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
    }

    public override void OnTriggerStay2D(Collider2D collision)
    {
    }

    protected void ApplyJumpForce(float jumpMultiplier)
    {
        Vector2 force = new(0f, stateManager.PlayerData.JumpForce * jumpMultiplier);
        stateManager.RB.linearVelocity = new(stateManager.RB.linearVelocityX, force.y);
        stateManager.JumpHangCounter = stateManager.PlayerData.JumpHangTime;
    }
    protected void Flip()
    {
        if (!CanFlip())
            return;
        if (stateManager.IsFacingRight)
        {
            stateManager.transform.rotation = Quaternion.Euler(new(stateManager.transform.rotation.x, 180f, stateManager.transform.rotation.z));
            stateManager.IsFacingRight = !stateManager.IsFacingRight;
        }
        else
        {
            stateManager.transform.rotation = Quaternion.Euler(new(stateManager.transform.rotation.x, 0.0f, stateManager.transform.rotation.z));
            stateManager.IsFacingRight = !stateManager.IsFacingRight;
        }
        bool CanFlip() => stateManager.IsFacingRight && UserInput.MoveVector.x < 0f || !stateManager.IsFacingRight && UserInput.MoveVector.x > 0f;
    }
    protected void Move(float speed, float acceleration, float deceleration)
    {
        //Speed player is trying to achieve
        float targetSpeed = UserInput.MoveVector.x * speed; 

        //Difference between targetSpeed and current speed
        float speedDif = targetSpeed - stateManager.RB.linearVelocityX;
        
        //If we are stopping, we use decceleration, else acceleration
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        //Force that should be applied to player
        float movement = speedDif * accelRate;

        stateManager.RB.AddForce(movement * Vector2.right);
    }

    protected void Jump(bool doubleJump)
    {
        if (!doubleJump)
        {
            // hasJumped = true;
            stateManager.LastGrounded = 0f;
        }
        else
            stateManager.RemainingJumps--;

        stateManager.LastJumpPressed = 0f;
        stateManager.JumpReleasedBuffer = 0f;
        stateManager.JumpCutTriggered = false;

        float multiplier = doubleJump ? stateManager.PlayerData.DoubleJumpMultiplier : 1f;
        ApplyJumpForce(multiplier);
    }
    protected void CheckJumpCut()
    {
        // jump cut (only after both jump types have been ruled out)
        if (stateManager.JumpReleasedBuffer > 0f && stateManager.RB.linearVelocityY > 0f)
        {
            stateManager.JumpCutTriggered = true;
            stateManager.JumpReleasedBuffer = 0f;
        }
    }
}