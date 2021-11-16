using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private PlayerControls _controls;
    private bool _isMoving;
    private bool _isLooking;

    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void Start()
    {
        RegisterKeysToFunctions();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void RegisterKeysToFunctions()
    {
        _controls.Player.Move.performed += OnMove;

        _controls.Player.Move.started += OnMoveStart;
        _controls.Player.Move.canceled += OnMoveEnd;

        _controls.Player.Look.performed += OnLook;
        _controls.Player.Look.started += OnLookStart;
        _controls.Player.Look.canceled += OnLookEnd;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Debug.Log($"OnMove {ctx.ReadValue<Vector2>()}");
    }

    private void OnMoveStart(InputAction.CallbackContext ctx)
    {
        _isMoving = true;
    }

    private void OnMoveEnd(InputAction.CallbackContext ctx)
    {
        _isMoving = false;
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        Debug.Log($"OnLook {ctx.ReadValue<Vector2>()}");
    }

    private void OnLookStart(InputAction.CallbackContext ctx)
    {
        _isLooking = true;
    }

    private void OnLookEnd(InputAction.CallbackContext ctx)
    {
        _isLooking = false;
    }
}