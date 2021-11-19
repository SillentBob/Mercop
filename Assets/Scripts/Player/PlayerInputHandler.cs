using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    
    [Header("Crosshair and Aim settings"), Space(5)]
    [SerializeField] private Image crosshairIcon;
    [SerializeField, FormerlySerializedAs("crosshairBounds")]
    private Vector2 crosshairMoveRange;
    [SerializeField] private Vector2 crosshairMoveSensitivity;

    private PlayerControls _controls;
    private bool _isMoving;
    private bool _isAiming;

    private Vector2 _currentMoveValues;
    private Vector2 _currentAimValues;

    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void Start()
    {
        RegisterKeysToFunctions();
    }

    private void Update()
    {
        if (_isMoving)
        {
            ProcessMoveInputs();
        }

        if (_isAiming)
        {
            ProcessAimInputs();
        }
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void ProcessMoveInputs()
    {
        var rotationInput = _currentMoveValues.x;
        var forwardInput = _currentMoveValues.y;
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
        if (_currentAimValues != Vector2.zero)
        {
            MoveCrosshair(_currentAimValues * Time.deltaTime);
        }
    }

    private void MoveCrosshair(Vector2 input)
    {
        crosshairIcon.transform.localPosition = GetClampedPosition(input);
    }

    private Vector3 GetClampedPosition(Vector2 input)
    {
        var localPosition = crosshairIcon.transform.localPosition;
        var xPos = Mathf.Clamp(localPosition.x + input.x * crosshairMoveSensitivity.x, -crosshairMoveRange.x,
            crosshairMoveRange.x);
        var yPos = Mathf.Clamp(localPosition.y + input.y * crosshairMoveSensitivity.y, -crosshairMoveRange.y,
            crosshairMoveRange.y);
        return new Vector3(xPos, yPos, 0);
    }

    private void RegisterKeysToFunctions()
    {
        _controls.Player.Move.performed += OnMove;
        _controls.Player.Move.started += OnMoveStart;
        _controls.Player.Move.canceled += OnMoveEnd;

        _controls.Player.Aim.performed += OnAim;
        _controls.Player.Aim.started += OnAimStart;
        _controls.Player.Aim.canceled += OnAimEnd;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        _currentMoveValues = ctx.ReadValue<Vector2>();
    }

    private void OnMoveStart(InputAction.CallbackContext ctx)
    {
        _isMoving = true;
    }

    private void OnMoveEnd(InputAction.CallbackContext ctx)
    {
        _isMoving = false;
    }

    private void OnAim(InputAction.CallbackContext ctx)
    {
        _currentAimValues = ctx.ReadValue<Vector2>();
    }

    private void OnAimStart(InputAction.CallbackContext ctx)
    {
        _isAiming = true;
    }

    private void OnAimEnd(InputAction.CallbackContext ctx)
    {
        _isAiming = false;
    }
}