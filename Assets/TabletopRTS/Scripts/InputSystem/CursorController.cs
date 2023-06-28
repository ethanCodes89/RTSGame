using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using TabletopRTS.Scripts.UnitBehavior;
using UnityEngine;
using UnityEngine.InputSystem;

public struct CursorControllerInputs
{
    public Vector2 MousePosition;
    public InputAction CursorPrimaryCommand;
    public InputAction CursorSecondaryCommand;
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
    private bool isSelecting;
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
        HandleFinalStateUpdates();
        
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
                break;
        }
    }

    private void HandleFinalStateUpdates()
    {
        if (Inputs.CursorSecondaryCommand.WasPressedThisFrame())
        {
            Ray ray = mainCamera.ScreenPointToRay(Inputs.MousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) &&
                hit.collider.gameObject
                    .TryGetComponent(out IUnit selectable)) //update this later with enemy class instead of selectable
            {
                return; //TODO:Update this later with setting to AttackCommand state
            }
            else
            {
                Inputs.CurrentState = CursorState.MoveCommand;
            }
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
        if (!Inputs.CursorPrimaryCommand.WasPerformedThisFrame() &&
            !Inputs.CursorSecondaryCommand.WasPerformedThisFrame()) return;
        
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
        }
        Inputs.CurrentState = CursorState.SelectCommand;
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
            isSelecting = false;
        }
        else if (Inputs.CursorPrimaryCommand.WasReleasedThisFrame())
        {
            SelectObject(Inputs.MousePosition);
            isSelecting = false;
        }
    }
    
    private void SelectObject(Vector2 mousePosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject.TryGetComponent(out IUnit selectable))
        {
            if (selectable.IsSelected && Inputs.IsShiftSelectEnabled)
            {
                selectable.IsSelected = false;
                ClearIndividualSelection(hit.collider.gameObject);
                return;
            }
            if(!Inputs.IsShiftSelectEnabled) ClearAllSelection();
            SelectedUnitsManager.CurrentSelection.Add(hit.collider.gameObject);
            selectable.IsSelected = true;
        }
    }

    private void SelectObjects(Vector2 startPosition, Vector2 endPosition)
    {
        if (startPosition == endPosition)
        {
            SelectObject(Inputs.MousePosition); //Incase of delayed click and release, ensure single selection occurs
            return;
        }
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
        
        var bounds = selectionBox.bounds;
        var colliders = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity);

        bool clearedSelectionThisFrame = false;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent(out IUnit selectable))
            {
                if (!Inputs.IsShiftSelectEnabled && !clearedSelectionThisFrame)
                {
                    ClearAllSelection();
                    clearedSelectionThisFrame = true;
                }
                if (!SelectedUnitsManager.CurrentSelection.Contains(collider.gameObject))
                {
                    SelectedUnitsManager.CurrentSelection.Add(collider.gameObject);
                    selectable.IsSelected = true;
                }
            }
        }
        
        Destroy(selectionBox, .02f);
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
        Vector2 topLeft = new Vector2(Mathf.Min(startPosition.x, endPosition.x), Mathf.Max(startPosition.y, endPosition.y));
        Vector2 topRight = new Vector2(Mathf.Max(startPosition.x, endPosition.x), Mathf.Max(startPosition.y, endPosition.y));
        Vector2 bottomLeft = new Vector2(Mathf.Min(startPosition.x, endPosition.x), Mathf.Min(startPosition.y, endPosition.y));
        Vector2 bottomRight = new Vector2(Mathf.Max(startPosition.x, endPosition.x), Mathf.Min(startPosition.y, endPosition.y));

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
}
