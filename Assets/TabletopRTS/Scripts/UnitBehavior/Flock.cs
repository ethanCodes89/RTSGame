using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TabletopRTS.Scripts.UnitBehavior
{
    public class Flock : MonoBehaviour
    {
        public FlockBehavior Behavior;
        
        private List<FlockAgent> agents = new List<FlockAgent>();
        private float maxSpeed;
        [Range(1f, 10f)] public float neighborRadius = 1.5f;
        [Range(0f, 1f)] public float avoidanceRadiusMultiplier = 0.5f;

        private float squareMaxSpeed;
        private float squareNeighborRadius;
        private float squareAvoidanceRadius;
        
        public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

        Flock(List<FlockAgent> flockAgents)
        {
            agents = flockAgents;
            foreach (FlockAgent agent in agents)
            {
                agent.SetFlock(this); //This will allow us to wipe any pre-existing flock data from the agent and let them switch flocks on the fly
            }
        }
        
        private void Start()
        {
            squareMaxSpeed = maxSpeed * maxSpeed;
            squareNeighborRadius = neighborRadius * neighborRadius;
            squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        }

        private void Update()
        {
            foreach (FlockAgent agent in agents)
            {
                List<Transform> context = GetNearbyObjects(agent);
                Vector3 move = Behavior.CalculateMove(agent, context, this);
                move = move.normalized * maxSpeed;
                agent.Move(move);
            }
        }

        private List<Transform> GetNearbyObjects(FlockAgent agent)
        {
            List<Transform> context = new List<Transform>();
            Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);

            foreach (Collider collider in contextColliders)
            {
                if (collider != agent.AgentCollider)
                {
                    context.Add(collider.transform);
                }
            }

            return context;
        }
    }   
}
