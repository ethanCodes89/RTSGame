using TabletopRTS.Scripts.UnitBehavior;
using UnityEngine;
using UnityEngine.InputSystem;

public struct CursorControllerInputs
{
    public Vector2 MousePosition;
    public InputAction CursorPrimaryCommand;
    public bool IsShiftSelectEnabled;
    public CursorState CurrentState;
}

public enum CursorState
{
    SelectCommand,
    MoveCommand,
    AttackCommand,
    DefendCommand,
    WaitCommand
}

public class CursorController : MonoBehaviour
{

    public SelectedUnitsManager SelectedUnitsManager;
    public CursorControllerInputs Inputs;
    public Texture2D CursorTexture;
    public Texture SelectionRectangle;
    private Camera mainCamera;
    private bool isSelecting = false;
    private Vector2  selectionStartPosition = Vector2.zero;
    private MeshCollider selectionBox;
    private Mesh selectionMesh;
    private Vector2[] selectionBoxCorners;
    private Vector3[] selectionBoxVertices;
    private void Awake()
    {
        Inputs = new CursorControllerInputs();
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
        Cursor.SetCursor(CursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        switch (Inputs.CurrentState)
        {
            case CursorState.SelectCommand:
                HandleObjectSelection();
                break;
            case CursorState.MoveCommand:
                HandleMoveCommand();
                break;
            default:
                Inputs.CurrentState = CursorState.SelectCommand;
                HandleObjectSelection();
                break;
        }

    }

    private void OnGUI()
    {
        if (Inputs.CursorPrimaryCommand.IsPressed() && isSelecting)
        {
            Rect selectionArea = CalculateSelectionArea(selectionStartPosition, Inputs.MousePosition);
            GUI.DrawTexture(selectionArea, SelectionRectangle);
        }
    }

    private void HandleMoveCommand()
    {
        if (!Inputs.CursorPrimaryCommand.WasPressedThisFrame())
            return;
        foreach (var unit in SelectedUnitsManager.CurrentSelection)
        {
            if (unit.TryGetComponent(out MoveComponent moveComponent))
            {
                Ray ray = mainCamera.ScreenPointToRay(Inputs.MousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 50000f, (1 << 8)))
                {
                    moveComponent.SetDestination(hit.point);   
                }
            }
            Inputs.CurrentState = CursorState.SelectCommand;
        }
    }

    private void HandleAttackCommand()
    {

    }

    private void HandleObjectSelection()
    {
        if (Inputs.CursorPrimaryCommand.WasPerformedThisFrame())
        {
            isSelecting = true;
            selectionStartPosition = Inputs.MousePosition;

        }
        else if (Inputs.CursorPrimaryCommand.WasReleasedThisFrame() && isSelecting)
        {
            SelectObjects(selectionStartPosition, Inputs.MousePosition);
        }
        else if (Inputs.CursorPrimaryCommand.WasReleasedThisFrame())
        {
            SelectObject(Inputs.MousePosition);
        }
    }
    
    private void SelectObject(Vector2 mousePosition)
    {
        if(!Inputs.IsShiftSelectEnabled) ClearAllSelection();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.TryGetComponent(out IUnit selectable))
        {
            if (selectable.IsSelected && Inputs.IsShiftSelectEnabled)
            {
                selectable.IsSelected = false;
                ClearIndividualSelection(hit.collider.gameObject);
                return;
            }
            selectable.IsSelected = true;
            SelectedUnitsManager.CurrentSelection.Add(hit.collider.gameObject);
        }
    }

    private void SelectObjects(Vector2 startPosition, Vector2 endPosition)
    {
        if(!Inputs.IsShiftSelectEnabled) ClearAllSelection();
        if (startPosition == endPosition) return; //if user did not move the mouse when selecting, return early to avoid issue with generating the rectangle
        var verts = new Vector3[4];
        int i = 0;
        var corners = GenerateRectangle(startPosition, endPosition);
        foreach (Vector2 corner in corners)
        {
            Ray ray = mainCamera.ScreenPointToRay(corner);
            if (Physics.Raycast(ray, out RaycastHit hit, 50000f, (1 << 8)))
            {
                verts[i] = new Vector3(hit.point.x, 0, hit.point.z); 
            }
            i++;
        }

        selectionMesh = GenerateSelectionMesh(verts);
        selectionBox = gameObject.AddComponent<MeshCollider>();
        selectionBox.sharedMesh = selectionMesh;
        selectionBox.convex = true;
        selectionBox.isTrigger = true;
        
        Destroy(selectionBox, .02f);
        isSelecting = false;
    }

    private void ClearAllSelection()
    {
        foreach (GameObject selectedObject in SelectedUnitsManager.CurrentSelection)
        {
            selectedObject.GetComponent<IUnit>().IsSelected = false;
        }
        SelectedUnitsManager.CurrentSelection.Clear();
    }

    private void ClearIndividualSelection(GameObject selectedUnit)
    {
        SelectedUnitsManager.CurrentSelection.Remove(selectedUnit);
    }

    private Rect CalculateSelectionArea(Vector2 startPosition, Vector2 endPosition)
    {
        Vector2 min = Vector2.Min(startPosition, endPosition);
        Vector2 max = Vector2.Max(startPosition, endPosition);
    
        //subtract from screen.height to convert the screen space coordinate system to the gui coordinate system - https://docs.unity3d.com/ScriptReference/Rect.html
        min.y = Screen.height - Mathf.Max(startPosition.y, endPosition.y);
        max.y = Screen.height - Mathf.Min(startPosition.y, endPosition.y);
    
        return new Rect(min, max - min);
    }
    
    private Vector2[] GenerateRectangle(Vector2 startPosition, Vector2 endPosition)
    {
        Vector2 topLeft;
        Vector2 topRight;
        Vector2 bottomLeft;
        Vector2 bottomRight;

        if (startPosition.x < endPosition.x)
        {
            if (startPosition.y > endPosition.y)
            {
                topLeft = startPosition;
                topRight = new Vector2(endPosition.x, startPosition.y);
                bottomLeft = new Vector2(startPosition.x, endPosition.y);
                bottomRight = endPosition;
            }
            else
            {
                topLeft = new Vector2(startPosition.x, endPosition.y);
                topRight = endPosition;
                bottomLeft = startPosition;
                bottomRight = new Vector2(endPosition.x, startPosition.y);
            }
        }
        else
        {
            if (startPosition.y > endPosition.y)
            {
                topLeft = new Vector2(endPosition.x, startPosition.y);
                topRight = startPosition;
                bottomLeft = endPosition;
                bottomRight = new Vector2(startPosition.x, endPosition.y);
            }
            else
            {
                topLeft = endPosition;
                topRight = new Vector2(startPosition.x, endPosition.y);
                bottomLeft = new Vector2(endPosition.x, startPosition.y);
                bottomRight = startPosition;
            }
        }

        Vector2[] corners = { topLeft, topRight, bottomLeft, bottomRight };
        return corners;
    }

    Mesh GenerateSelectionMesh(Vector3[] corners)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + Vector3.up * 100f;
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;
        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IUnit selectable))
        {
            if (!SelectedUnitsManager.CurrentSelection.Contains(other.gameObject))
            {
                SelectedUnitsManager.CurrentSelection.Add(other.gameObject);
                selectable.IsSelected = true;   
            }
        }
    }
}
