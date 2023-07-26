using TabletopRTS.UnitBehavior;
using UnityEngine;

namespace TabletopRTS.Flocking
{
    [RequireComponent(typeof(Collider))]
    public class FlockAgent : MonoBehaviour
    {
        private Flock agentFlock;
        public Flock AgentFlock { get { return agentFlock; } }
        private float speed;
        public float Speed { get { return speed; } }
        private Collider agentCollider;
        public Collider AgentCollider { get { return agentCollider; } }
        private void Start()
        {
            agentCollider = GetComponent<Collider>();
            speed = GetComponent<IUnit>().Speed;
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