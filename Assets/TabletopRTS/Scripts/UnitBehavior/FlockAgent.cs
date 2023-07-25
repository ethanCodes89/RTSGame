using UnityEngine;

namespace TabletopRTS.Scripts.UnitBehavior
{
    [RequireComponent(typeof(Collider))]
    public class FlockAgent : MonoBehaviour
    {
        private Flock agentFlock;
        public Flock AgentFlock { get { return agentFlock; } }
        private Collider agentCollider;
        public Collider AgentCollider { get { return agentCollider; } }
        private void Start()
        {
            agentCollider = GetComponent<Collider>();
        }

        public void SetFlock(Flock flock)
        {
            agentFlock = flock;
        }

        public void Move(Vector3 velocity)
        {
            transform.forward = velocity;
            transform.position += velocity * Time.deltaTime;
        }
    }
}