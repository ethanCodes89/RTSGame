using System.Collections.Generic;
using TabletopRTS.UnitBehavior;
using UnityEngine;

namespace TabletopRTS.Flocking
{
    public abstract class ContextFilter : ScriptableObject
    {
        public abstract List<Transform> Filter(FlockAgent agent, List<Transform> original);
    }   
}
