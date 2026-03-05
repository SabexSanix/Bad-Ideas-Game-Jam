using UnityEngine;

public class PlayerStateManager : StateManager<PlayerStateManager.PlayerState>
{
    public static PlayerStateManager Instance { get; private set;}

    [SerializeField] private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    [SerializeField] private  bool isFacingRight = true;
    public bool IsFacingRight { get { return isFacingRight; }  set { isFacingRight = value; } }

    //Jump
    private float lastGrounded;
    private float lastJumpPressed;
    private float jumpHangCounter;
    private float jumpReleasedBuffer;
    private bool jumpCutTriggered;
    private int remainingJumps;

    public float LastGrounded {get { return lastGrounded; } set { lastGrounded = value;}}
    public float LastJumpPressed {get { return lastJumpPressed; } set { lastJumpPressed = value;}}
    public float JumpHangCounter {get { return jumpHangCounter;} set { jumpHangCounter = value;}}
    public float JumpReleasedBuffer {get { return jumpReleasedBuffer; } set { jumpReleasedBuffer = value;}}
    public bool JumpCutTriggered {get { return jumpCutTriggered; } set { jumpCutTriggered = value;}}
    public int RemainingJumps { get { return remainingJumps; } set { remainingJumps = value; } }

    //Dash
    private bool isLeaping;
    private bool canLeap = true;

    public bool IsLeaping { get { return isLeaping; } set { isLeaping = value; } }
    public bool CanLeap {get { return canLeap;} set { canLeap = value; } }

    [Header("Checks")]

    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new(0.9f, 0.1f);
    [SerializeField] private LayerMask groundLayers;

    public bool IsGrounded
    {
        get { return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayers); }
    }

    [Header("Other")]

    [SerializeField] private Rigidbody2D rb;

    public Rigidbody2D RB => rb;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Player State Manager in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        rb = GetComponent<Rigidbody2D>();

        States.Add(PlayerState.Idle, new PlayerIdleState(PlayerState.Idle, this));
        States.Add(PlayerState.Crawl, new PlayerCrawlState(PlayerState.Crawl, this));
        States.Add(PlayerState.Walk, new PlayerWalkState(PlayerState.Walk, this));
        States.Add(PlayerState.Run, new PlayerRunState(PlayerState.Run, this));
        States.Add(PlayerState.Jump, new PlayerJumpState(PlayerState.Jump, this));
        States.Add(PlayerState.DoubleJump, new PlayerDoubleJumpState(PlayerState.DoubleJump, this));
        States.Add(PlayerState.Leap, new PlayerLeapState(PlayerState.Leap, this));
        States.Add(PlayerState.Fall, new PlayerFallState(PlayerState.Fall, this));

        if (!States.TryGetValue(IsGrounded ? PlayerState.Idle : PlayerState.Fall, out var startState))
        {
            Debug.LogError("Start state not found in dictionary!");
            return;
        }

        CurrentState = startState;
    }

    protected override void Update()
    {
        ManageTimers();

        base.Update();
    }
    protected override void FixedUpdate()
    {
        if (CurrentState.StateKey != PlayerState.Leap)
        {
            Gravity();
            ApplyFriction();
        }
        JumpHangCounter = Mathf.Max(0f, JumpHangCounter - Time.fixedDeltaTime);
        base.FixedUpdate();
    }
    private void ManageTimers()
    {
        if (IsGrounded)
        {
            lastGrounded = playerData.LastGroundedTime;
            jumpCutTriggered = false;
            remainingJumps = playerData.ExtraJumpAmount;
        }
        else
        {
            lastGrounded -= Time.deltaTime;
            if (lastGrounded < 0) lastGrounded = 0;
        }
        if (UserInput.WasJumpPressed)
        {
            lastJumpPressed = playerData.LastJumpPressedTime;
        }
        else
        {
            lastJumpPressed -= Time.deltaTime;
            if (lastJumpPressed < 0) lastJumpPressed = 0;
        }
        if (UserInput.WasJumpReleased)
        {
            jumpReleasedBuffer = playerData.JumpReleasedBufferTime;
        }
        else
        {
            jumpReleasedBuffer -= Time.deltaTime;
            if (jumpReleasedBuffer < 0) jumpReleasedBuffer = 0;
        }
    }
    private void Gravity()
    {
        if (jumpHangCounter > 0f)
        {
            rb.gravityScale = playerData.GravityScale;
            return;
        }
        if (jumpCutTriggered && rb.linearVelocityY > 0f)
        {
                //Increase gravity to shorten the jump (Variable jump height)
                rb.gravityScale = playerData.GravityScale * playerData.JumpCutGravityMultiplier;
        }
            else if (rb.linearVelocityY < 0)
            {
                rb.gravityScale = playerData.GravityScale * playerData.FallGravityMultiplier;
                rb.linearVelocity = new(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -playerData.MaxFallSpeed));
            }
            else
            {
                rb.gravityScale = playerData.GravityScale;
            }
    }
    void ApplyFriction()
    {
        if(Mathf.Abs(UserInput.MoveVector.x) < 0.01f) //If we are trying to stop
        {
            //Get friction amount
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocityX), Mathf.Abs(playerData.FrictionAmount));
            amount *= Mathf.Sign(rb.linearVelocityX); //align the direction
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse); //Apply it in opposite direction
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
    public enum PlayerState
    {
        Idle,
        Crawl,
        Walk,
        Run,
        Jump,
        DoubleJump,
        Leap,
        Fall
    }
}