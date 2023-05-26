using System;
using TabletopRTS.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputValues : MonoBehaviour
{
    private InputActions inputActions;
    private InputAction movement;
    private InputAction mousePosition;
    private InputAction zoomCamera;
    private InputAction dragCamera;
    private InputAction selectObject;
    private InputAction shiftSelectEnabled;
    private void Awake()
    {
        inputActions = new InputActions();
        movement = inputActions.GameplayActions.MoveCamera;
        mousePosition = inputActions.GameplayActions.MousePosition;
        zoomCamera = inputActions.GameplayActions.ZoomCamera;
        dragCamera = inputActions.GameplayActions.DragCamera;
        selectObject = inputActions.GameplayActions.SelectSingleObject;
        shiftSelectEnabled = inputActions.GameplayActions.ShiftSelectEnabled;
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
    public InputAction SelectObject { get { return selectObject; } }
    public bool ShiftSelectEnabled { get { return shiftSelectEnabled.IsPressed(); } }
}
