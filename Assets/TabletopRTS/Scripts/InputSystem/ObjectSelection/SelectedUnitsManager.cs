using System.Collections.Generic;
using TabletopRTS.Scripts.UnitBehavior;
using UnityEngine;

public class SelectedUnitsManager : MonoBehaviour
{
    public List<GameObject> CurrentSelection;
    private void Start()
    {
        CurrentSelection = new List<GameObject>();
    }
}
