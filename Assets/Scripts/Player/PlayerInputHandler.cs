using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // @formatter:off
    
    [SerializeField] private PlayerController playerController;

    [Header("Crosshair and Aim settings"), Space(5)] 
    [SerializeField] private Vector2 crosshairMoveRange;
    [SerializeField] private Vector2 crosshairMoveSensitivity;

     // @formatter:on
    
    private PlayerControls controls;
    private bool isMoving;
    private bool isAiming;

    private Vector2 currentMoveValues;
    private Vector2 currentAimValues;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        RegisterKeysToFunctions();
    }

    private void Update()
    {
        if (isMoving)
        {
            ProcessMoveInputs();
        }

        if (isAiming)
        {
            ProcessAimInputs();
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void ProcessMoveInputs()
    {
        var rotationInput = currentMoveValues.x;
        var forwardInput = currentMoveValues.y;
        if (rotationInput != 0)
        {
            playerController.Rotate(new Vector3(0, rotationInput * Time.deltaTime, 0));
        }

        if (forwardInput != 0)
        {
            playerController.Move(new Vector3(0, forwardInput * Time.deltaTime, 0));
        }
    }

    private void ProcessAimInputs()
    {
        if (currentAimValues != Vector2.zero)
        {
            MoveCrosshair(currentAimValues * Time.deltaTime);
        }
    }

    private void MoveCrosshair(Vector2 input)
    {
        PlayerGuiManager.Instance.MoveCrosshair(input, crosshairMoveSensitivity, crosshairMoveRange);
    }

    private void RegisterKeysToFunctions()
    {
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.started += OnMoveStart;
        controls.Player.Move.canceled += OnMoveEnd;

        controls.Player.Aim.performed += OnAim;
        controls.Player.Aim.started += OnAimStart;
        controls.Player.Aim.canceled += OnAimEnd;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        currentMoveValues = ctx.ReadValue<Vector2>();
    }

    private void OnMoveStart(InputAction.CallbackContext ctx)
    {
        isMoving = true;
    }

    private void OnMoveEnd(InputAction.CallbackContext ctx)
    {
        isMoving = false;
    }

    private void OnAim(InputAction.CallbackContext ctx)
    {
        currentAimValues = ctx.ReadValue<Vector2>();
    }

    private void OnAimStart(InputAction.CallbackContext ctx)
    {
        isAiming = true;
    }

    private void OnAimEnd(InputAction.CallbackContext ctx)
    {
        isAiming = false;
    }
}