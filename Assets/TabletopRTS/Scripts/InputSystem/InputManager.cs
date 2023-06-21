using System;
using UnityEngine;
using Sirenix.OdinInspector;
public class InputManager : MonoBehaviour
{
    public InputValues InputValuesReference;
    public CursorController CursorController;
    public CameraController CameraController;

    private void Start()
    {
        //Set initial values
        PullCursorControllerInputs();
        PullCameraControllerInputs();
    }

    private void Update()
    {
        PullCursorControllerInputs();
        PullCameraControllerInputs();
    }

    private void PullCameraControllerInputs()
    {
        CameraController.Inputs.DragCameraJustPressed = InputValuesReference.DragCamera.WasPressedThisFrame();
        CameraController.Inputs.DragCameraIsPressed = InputValuesReference.DragCamera.IsPressed();
        CameraController.Inputs.ZoomCamera = InputValuesReference.ZoomCamera;
        CameraController.Inputs.MousePosition = InputValuesReference.MousePosition;
        CameraController.Inputs.Movement = InputValuesReference.Movement;
    }
    private void PullCursorControllerInputs()
    {
        CursorController.Inputs.CursorPrimaryCommand = InputValuesReference.CursorPrimaryCommand;
        CursorController.Inputs.MousePosition = InputValuesReference.MousePosition;
        CursorController.Inputs.IsShiftSelectEnabled = InputValuesReference.ShiftSelectEnabled;
        HandleCursorState();
    }

    private void HandleCursorState()
    {
        if (InputValuesReference.SetCursorMoveState)
        {
            CursorController.Inputs.CurrentState = CursorState.MoveCommand;
        }
    }
    
}
