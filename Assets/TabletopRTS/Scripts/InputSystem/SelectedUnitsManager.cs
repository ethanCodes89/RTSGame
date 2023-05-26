using System.Collections.Generic;
using UnityEngine;

public class SelectedUnitsManager : MonoBehaviour
{
    public List<BaseSelectable> CurrentSelection;
    private void Start()
    {
        CurrentSelection = new List<BaseSelectable>();
    }
}
