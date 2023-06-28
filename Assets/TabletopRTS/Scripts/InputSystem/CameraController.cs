//Code came from: https://www.youtube.com/watch?v=3Y7TFN_DsoI
//Referencing for future explanation
using Sirenix.OdinInspector;
using UnityEngine;

public struct CameraControllerInputs
{
    public Vector2 Movement;
    public Vector2 MousePosition;
    public Vector2 ZoomCamera;
    public bool DragCameraJustPressed;
    public bool DragCameraIsPressed;
}

public class CameraController : MonoBehaviour
{
    public CameraControllerInputs Inputs;
    [SerializeField] private Camera mainCamera;
    private Transform cameraTransform;

    [TitleGroup("Horizontal Motion")] //X,Z motion
    [SerializeField] private float maxSpeed = 5f;
    private float speed;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float damping = 15;

    [Space] [TitleGroup("Vertical Motion")] //Y motion
    [SerializeField] private float stepSize = 2f;
    [SerializeField] private float zoomDampening = 7.5f;
    [SerializeField] private float minHeight = 5f;
    [SerializeField] private float maxHeight = 50f;
    [SerializeField] private float zoomSpeed = 2f;

    [Space] [TitleGroup("Screen Edge Motion")] 
    [SerializeField] [Range(0f, .1f)] private float edgeTolerance = 0.05f;
    [SerializeField] private bool useScreenEdge = true;
    
    private Vector3 targetPosition;
    private float zoomHeight;
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;
    private Vector3 startDrag;

    private void Awake()
    {
        cameraTransform = mainCamera.transform;
    }

    private void Start()
    {
        Inputs = new CameraControllerInputs();
    }

    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(transform);
        lastPosition = transform.position;
    }

    private void Update()
    {
        GetKeyboardMovement();
        UpdateCameraPosition();
        ZoomCamera(Inputs.ZoomCamera);
        if(useScreenEdge)
            CheckCursorAtScreenEdge();
        DragCamera();
        UpdateVelocity();
        UpdateBasePosition();
    }

    private void UpdateVelocity()
    {
        horizontalVelocity = (transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0; //set Y velocity to 0 because we don't want any vertical movement
        lastPosition = transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = Inputs.Movement.x * GetCameraRight() +
                             Inputs.Movement.y * GetCameraForward();
        inputValue = inputValue.normalized; //TODO: for more understanding, check how this value changes with different Inputs

        if (inputValue.sqrMagnitude > 0.1f)
            targetPosition += inputValue;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right; //TODO: for more understanding, check how this value changes with different camera directions
        right.y = 0;
        return right;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward; //TODO: for more understanding, check how this value changes with different camera directions
        forward.y = 0;
        return forward;
    }

    private void UpdateBasePosition() //TODO: for more understanding, output values in this functions and step through to watch how it works.
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        targetPosition = Vector3.zero;
    }

    private void ZoomCamera(Vector2 inputValue)
    {
        float value = -inputValue.y / 100f;
        if (Mathf.Abs(value) > 0.1f)
        {
            zoomHeight = Mathf.Clamp(cameraTransform.localPosition.y + value * stepSize, minHeight, maxHeight);
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        cameraTransform.LookAt(transform);
    }

    private void CheckCursorAtScreenEdge()
    {
        Vector2 cursorPosition = Inputs.MousePosition;
        Vector3 moveDirection = Vector3.zero;

        if (cursorPosition.x < edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (cursorPosition.x > (1 - edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();
        
        if (cursorPosition.y < edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if (cursorPosition.y > (1 - edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        targetPosition += moveDirection;
    }

    private void DragCamera()
    {
        if (!Inputs.DragCameraIsPressed)
            return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCamera.ScreenPointToRay(Inputs.MousePosition);

        if (plane.Raycast(ray, out float distance))
        {
            if (Inputs.DragCameraJustPressed)
                startDrag = ray.GetPoint(distance);
            else
                targetPosition += startDrag - ray.GetPoint(distance);

        }
    }
}