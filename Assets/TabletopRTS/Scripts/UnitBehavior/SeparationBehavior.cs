using System.Collections.Generic;
using UnityEngine;

namespace TabletopRTS.Flocking
{
    [CreateAssetMenu(menuName = "Flock/Behavior/Separation")]
    public class SeparationBehavior : FilteredFlockBehavior
    {
        public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
        {
            if (context.Count == 0)
                return Vector3.zero;

            Vector3 separationMove = Vector3.zero;
            int separate = 0;
            List<Transform> filteredContext = filter == null ? context : filter.Filter(agent, context);
            foreach (Transform item in filteredContext)
            {
                if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
                {
                    separate++;
                    separationMove += agent.transform.position - item.position;
                }
            }

            if (separate > 0)
                separationMove /= separate;
        
            return separationMove;
        }
    }   
}
