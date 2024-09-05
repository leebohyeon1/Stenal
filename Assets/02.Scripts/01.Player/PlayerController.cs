using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveInput { get; private set; }
    public bool attackInput { get; private set; }
    public bool defendInput { get; private set; }
    public bool dodgeInput { get; private set; }
    public bool interactInput { get; private set; }
    public bool pauseInput { get; private set; }  
    public bool recoverHealthInput { get; private set; }

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction defendAction;
    private InputAction dodgeAction;
    private InputAction interactAction;
    private InputAction pauseAction;
    private InputAction recoverHealthAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        SetUpInputActions();
    }

    private void OnEnable()
    {
        //playerInput.Enable();
    }

    private void OnDisable()
    {
        //playerInput.Disable();
    }

    private void Update()
    {
        // HandleMovement();
        HandleActions();
    }

    private void SetUpInputActions()
    {
        moveAction = playerInput.actions["Move"];
        attackAction = playerInput.actions["Attack"];
        defendAction = playerInput.actions["Defend"];
        dodgeAction = playerInput.actions["Dodge"];
        interactAction = playerInput.actions["Interact"];
        pauseAction = playerInput.actions["Pause"];
        recoverHealthAction = playerInput.actions["RecoverHealth"];
    }

    private void HandleActions()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        attackInput = attackAction.WasPressedThisFrame();

        defendInput = defendAction.IsPressed();

        dodgeInput = dodgeAction.WasPressedThisFrame();

        interactInput = interactAction.WasPressedThisFrame();

        pauseInput = pauseAction.WasPressedThisFrame();

        recoverHealthInput = recoverHealthAction.WasPressedThisFrame();
    }

}
