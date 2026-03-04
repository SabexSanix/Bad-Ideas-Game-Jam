using UnityEngine;

public class UserInput : MonoBehaviour
{
    public static UserInput Instance { get; private set; }
    public static PlayerInput PlayerInput;

    public static Vector2 MoveVector;
    public static bool WasJumpPressed;
    public static bool IsJumpBeingPressed;
    public static bool WasJumpReleased;
    public static bool WasLeapHoldPressed;
    public static bool IsLeapHoldBeingPressed;
    public static bool WasLeapHoldReleased;
    public static bool WasRunPressed;
    public static bool IsRunBeingPressed;
    public static bool WasAttackPressed;
    public static bool WasCrawlPressed;
    public static bool IsCrawlBeingPressed;
    public static bool WasInteractPressed;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one User Input in the scene. Destroying the newest one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PlayerInput = new PlayerInput();
    }

    private void Update()
    {
        MoveVector = PlayerInput.Player.Move.ReadValue<Vector2>();
        WasJumpPressed = PlayerInput.Player.Jump.WasPressedThisFrame();
        IsJumpBeingPressed = PlayerInput.Player.Jump.IsPressed();
        WasJumpReleased = PlayerInput.Player.Jump.WasReleasedThisFrame();
        WasRunPressed = PlayerInput.Player.Run.WasPressedThisFrame();
        IsRunBeingPressed = PlayerInput.Player.Run.IsPressed();
        WasLeapHoldPressed = PlayerInput.Player.Leap.WasPressedThisFrame();
        IsLeapHoldBeingPressed = PlayerInput.Player.Leap.IsPressed();
        WasLeapHoldReleased = PlayerInput.Player.Leap.WasReleasedThisFrame();
        WasCrawlPressed = PlayerInput.Player.Crawl.WasPressedThisFrame();
        IsCrawlBeingPressed = PlayerInput.Player.Crawl.IsPressed();
        WasAttackPressed = PlayerInput.Player.Attack.WasPressedThisFrame();
        WasInteractPressed = PlayerInput.Player.Interact.WasPressedThisFrame();
    }
    public static void EnableInput()
    {
        PlayerInput.Enable();
    }
    public static void DisableInput()
    {
        PlayerInput.Disable();
        
        //reset values
        MoveVector = Vector2.zero;
        WasJumpPressed = false;
        IsJumpBeingPressed = false;
        WasJumpReleased = false;
        WasRunPressed = false;
        IsRunBeingPressed = false;
        WasLeapHoldPressed = false;
        IsLeapHoldBeingPressed = false;
        WasLeapHoldReleased = false;
        WasCrawlPressed = false;
        IsCrawlBeingPressed = false;
        WasAttackPressed = false;
        WasInteractPressed = false;
    }
    private void OnEnable()
    {
        EnableInput();
    }
    private void OnDisable()
    {
        DisableInput();
    }
}
