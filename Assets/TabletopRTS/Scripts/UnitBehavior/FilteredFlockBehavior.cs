using System.Collections;
using System.Collections.Generic;
using TabletopRTS.Scripts.UnitBehavior;
using UnityEngine;

public abstract class FilteredFlockBehavior : FlockBehavior
{
    public ContextFilter filter;
}
