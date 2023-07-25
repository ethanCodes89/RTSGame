using System.Collections;
using System.Collections.Generic;
using TabletopRTS.Scripts.UnitBehavior;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")]
public class CohesionBehavior : FilteredFlockBehavior
{
    public float AgentSmoothTime = 0.5f;
    private Vector3 currentVelocity;
    
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if(context.Count == 0)
            return Vector3.zero;

        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = filter == null ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += item.position;
        }

        cohesionMove /= context.Count;
        cohesionMove -= agent.transform.position;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, AgentSmoothTime);
        return cohesionMove;
    }
}
