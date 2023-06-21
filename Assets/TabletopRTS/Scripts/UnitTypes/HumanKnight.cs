using System.Collections;
using System.Collections.Generic;
using TabletopRTS.Scripts.UnitBehavior;
using UnityEngine;

public class HumanKnight : MonoBehaviour, IUnit
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float speed;
    private bool isSelected;
    
    public float Health { get { return health;  } set { health = value; } } 
    public float Speed { get { return speed;  } set { speed = value; } }
    public bool IsSelected { get { return isSelected;  } set { isSelected = value; } }
}
