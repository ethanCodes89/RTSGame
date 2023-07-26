using System.Collections.Generic;
using UnityEngine;

namespace TabletopRTS.Flocking
{
    [CreateAssetMenu(menuName = "Flock/Filter/Physics Layer")]
    public class PhysicsLayerFilter : ContextFilter
    {
        public LayerMask Mask;

        public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
        {
            List<Transform> filtered = new List<Transform>();
            foreach (Transform item in original)
            {
                if (Mask == (Mask | (1 << item.gameObject.layer)))
                {
                    filtered.Add(item);
                }
            }

            return filtered;
        }
    }   
}
