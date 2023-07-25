using System.Collections;
using System.Collections.Generic;
using TabletopRTS.Scripts.UnitBehavior;
using UnityEngine;
[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    public FlockBehavior[] Behaviors;
    public float[] weights;
    
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (weights.Length != Behaviors.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector2.zero;   
        }

        Vector3 move = Vector3.zero;

        for (int i = 0; i < Behaviors.Length; i++)
        {
            Vector3 partialMove = Behaviors[i].CalculateMove(agent, context, flock) * weights[i];

            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;
    }
}
