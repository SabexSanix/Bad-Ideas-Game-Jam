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

    [Header("Movement")]
    [Space(5f)]
    public MovementData CrawlData = new(3f, 3f, 3f);
    public MovementData WalkData = new(7f, 6.9f, 6.9f);
    public MovementData RunData = new(9f, 8.4f, 8.7f);
    public float FrictionAmount = 0.2f;

    [Header("Jump Assists")]
    [Space(5f)]
    public float LastGroundedTime = 0.15f;
    public float LastJumpPressedTime = 0.1f;
    public float JumpHangTime = 0.1f;
    public float JumpReleasedBufferTime = 0.1f;

    [Header("Jump")]
    [Space(5f)]
    public float JumpForce = 14f;
    public float JumpCutGravityMultiplier = 3f; //If jump released early, gravity increases, shortening the jump

    [Header("Double Jump")]
    [Space(5f)]
    public int ExtraJumpAmount = 1;
    [Range(0.0f, 1f)]
    public float DoubleJumpMultiplier = 0.75f;

    [Header("Dash")]
    [Space(5f)]
    public Vector2 LeapForce = new Vector2(20f, 7f);
    public float LeapTime = 0.2f;
    public float DashCoolDown = 0.75f;

}
 
[System.Serializable]
public class MovementData
{
    public float Speed;
    public float Acceleration;
    public float Deceleration;

    public MovementData(float speed, float acceleration, float deceleration)
    {
        Speed = speed;
        Acceleration = acceleration;
        Deceleration = deceleration;
    }    
}