using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction defendAction;
    private InputAction dodgeAction;
    private InputAction interactAction;

    public Vector2 moveInput;
    public bool attackInput;
    public bool defendInput;
    public bool dodgeInput;
    public bool interactInput;
   
    private void Awake()
    {
        playerInput = new PlayerInput();  // Input Action Asset에서 생성한 클래스

        // 액션 할당
        moveAction = playerInput.Player.Move;
        attackAction = playerInput.Player.Attack;
        defendAction = playerInput.Player.Defend;
        dodgeAction = playerInput.Player.Dodge;
        interactAction = playerInput.Player.Interact;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        // HandleMovement();
        HandleActions();
    }

    private void HandleActions()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        attackInput = attackAction.triggered;

        if (defendAction.ReadValue<float>() > 0)
        {
            defendInput = true;
        }
        else
        {
            defendInput = false;
        }

        dodgeInput = dodgeAction.triggered;

        interactInput = interactAction.triggered;
    }


}
