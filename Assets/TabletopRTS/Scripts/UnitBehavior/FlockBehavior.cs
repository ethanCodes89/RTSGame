using System.Collections.Generic;
using UnityEngine;

namespace TabletopRTS.Scripts.UnitBehavior
{
    public abstract class FlockBehavior : ScriptableObject
    {
        public abstract Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock);
    }
}