using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    [Space(5f)]
    public float MaxFallSpeed = 12f;
    public float GravityScale = 1f;
    public float FallGravityMultiplier = 1.9f; //Higher Gravity When Falling

    [Space(5f)]

    [Header("Run")]
    [Space(5f)]
    public float Speed = 5f;
    public float Acceleration = 4.5f;
    public float Deceleration = 4f;

    [Header("Jump Assists")]
    [Space(5f)]
    public float LastGroundedTime = 0.15f;
    public float LastJumpPressedTime = 0.1f;
    public float JumpHangTime = 0.1f;
    public float JumpReleasedBufferTime = 0.1f;

    [Header("Jump")]
    [Space(5f)]
    public float JumpForce = 7f;
    public float JumpCutGravityMultiplier = 3f; //If jump released early, gravity increases, shortening the jump

    [Header("Double Jump")]
    [Space(5f)]
    public int ExtraJumpAmount = 1;
    [Range(0.0f, 1f)]
    public float DoubleJumpMultiplier = 0.75f;

    [Header("Dash")]
    [Space(5f)]
    public float LeapForce = 25f;
    public float LeapTime = 0.1f;
    public float DashCoolDown = 0.75f;

}
 