using UnityEngine;
using UnityEngine.InputSystem;

public class InputValues : MonoBehaviour
{
    private InputActions inputActions;
    private InputAction movement;
    private InputAction mousePosition;
    private InputAction zoomCamera;
    private InputAction dragCamera;
    private InputAction primaryCursorCommand;
    private InputAction secondaryCursorCommand;
    private InputAction shiftSelectEnabled;
    private InputAction setCursorMoveState;
    private void Awake()
    {
        inputActions = new InputActions();
        movement = inputActions.GameplayActions.MoveCamera;
        mousePosition = inputActions.GameplayActions.MousePosition;
        zoomCamera = inputActions.GameplayActions.ZoomCamera;
        dragCamera = inputActions.GameplayActions.DragCamera;
        primaryCursorCommand = inputActions.GameplayActions.PrimaryCursorCommand;
        secondaryCursorCommand = inputActions.GameplayActions.SecondaryCursorCommand;
        shiftSelectEnabled = inputActions.GameplayActions.ShiftSelectEnabled;
        setCursorMoveState = inputActions.GameplayActions.SetCursorMoveState;
    }

    private void OnEnable()
    {
        inputActions.GameplayActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public Vector2 Movement { get { return movement.ReadValue<Vector2>(); } }
    public Vector2 MousePosition { get { return mousePosition.ReadValue<Vector2>(); } }
    public Vector2 ZoomCamera { get { return zoomCamera.ReadValue<Vector2>(); } }
    public InputAction DragCamera { get { return dragCamera; } }
    public InputAction PrimaryCursorCommand { get { return primaryCursorCommand; } }
    public InputAction SecondaryCursorCommand { get { return secondaryCursorCommand; } }
    public bool ShiftSelectEnabled { get { return shiftSelectEnabled.IsPressed(); } }
    public bool SetCursorMoveState { get { return setCursorMoveState.WasPerformedThisFrame(); } }
}
