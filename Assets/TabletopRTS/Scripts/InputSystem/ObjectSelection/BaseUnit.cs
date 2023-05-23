using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BaseUnit : NetworkBehaviour
{
    private bool isSelected;
    
    public bool IsSelected { get { return isSelected; } set { isSelected = value; } }
}
