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

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction defendAction;
    private InputAction dodgeAction;
    private InputAction interactAction;
    private InputAction pauseAction;

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
    }

    private void HandleActions()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        attackInput = attackAction.WasPressedThisFrame();

        defendInput = defendAction.IsPressed();

       dodgeInput = dodgeAction.WasPressedThisFrame();

        interactInput = interactAction.WasPressedThisFrame();

        if(pauseAction.triggered)
        {
            GameManager.Instance.PauseControl();
        }
    }

}
