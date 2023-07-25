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
    //TODO: create UnitGroup structs or SingleUnit structs and set them up to run their course.
    //TODO: add a function to sort the CurrentSelection list based on Unit ranks
}
