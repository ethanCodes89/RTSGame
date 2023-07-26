using System.Collections.Generic;
using UnityEngine;

namespace TabletopRTS.Flocking
{
    [CreateAssetMenu(menuName = "Flock/Behavior/Stay In Radius")]
    public class FollowDominantPathBehavior : FlockBehavior
    {
        public Vector3 Center;
        public float Radius = 5f;

        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            Vector3 CenterOffset = Center - agent.transform.position;
            float t = CenterOffset.magnitude / Radius;

            if (t < 0.9f)
            {
                return Vector3.zero;
            }

            return CenterOffset * t * t;
        }
    }   
}
