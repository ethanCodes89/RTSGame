using System.Collections.Generic;
using Sirenix.OdinInspector.Editor.Examples;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public struct CursorControllerInputs
{
    public Vector2 MousePosition;
    public InputAction SelectObject;
}

public class CursorController : MonoBehaviour
{
    public CursorControllerInputs Inputs;
    public Transform CursorObjectTransform;
    public Texture SelectionRectangle;
    private Camera mainCamera;
    private List<GameObject> currentSelection = new List<GameObject>();
    private bool isSelecting = false;
    private Vector2  selectionStartPosition = Vector2.zero;
    public List<GameObject> AllUnits = new List<GameObject>();
    private void Awake()
    {
        Inputs = new CursorControllerInputs();
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        CursorObjectTransform.position = Inputs.MousePosition;

        if (Inputs.SelectObject.WasPerformedThisFrame())
        {
            isSelecting = true;
            selectionStartPosition = Inputs.MousePosition;

        }
        else if (Inputs.SelectObject.WasReleasedThisFrame() && isSelecting)
        {
            SelectObjects(selectionStartPosition, Inputs.MousePosition);
        }
        else if (Inputs.SelectObject.WasReleasedThisFrame())
        {
            SelectObject();
        }
    }

    private void OnGUI()
    {
        if (Inputs.SelectObject.IsPressed() && isSelecting)
        {
            Rect selectionArea = CalculateSelectionArea(selectionStartPosition, Inputs.MousePosition);
            GUI.DrawTexture(selectionArea, SelectionRectangle);
        }
    }

    private GameObject CheckForSelectable(Vector2 mousePosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.GetComponent<BaseUnit>())
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    private void SelectObject()
    {
        
        GameObject hitObject = CheckForSelectable(Inputs.MousePosition);
        ClearSelection();
        if (hitObject)
        {
            hitObject.GetComponent<BaseUnit>().IsSelected = true;
            currentSelection.Add(hitObject);
            return;
        }
    }

    private void SelectObjects(Vector2 startPosition, Vector2 endPosition)
    {
        ClearSelection();
        Rect rectangle = CalculateSelectionArea(startPosition, endPosition);
        foreach (var unit in AllUnits)
        {
            if (rectangle.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
            {
                if (!currentSelection.Contains(unit))
                {
                    currentSelection.Add(unit);
                    unit.GetComponent<BaseUnit>().IsSelected = true;
                }
            }
        }
        isSelecting = false;
    }

    private void ClearSelection()
    {
        foreach (GameObject selectedObject in currentSelection)
        {
            if (selectedObject != null)
            {
                selectedObject.GetComponent<BaseUnit>().IsSelected = false;
            }
        }

        currentSelection.Clear();
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

}
