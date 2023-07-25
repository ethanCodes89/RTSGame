using System;
using TabletopRTS.Scripts.UnitBehavior;
using UnityEngine;
using UnityEngine.AI;

public class MoveComponent : MonoBehaviour
{
    private float speed;
    private Vector3 currentDestination;
    private NavMeshAgent agent;
    private void Start()
    {
        var unit = GetComponent<IUnit>();
        speed = unit.Speed;
        currentDestination = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (currentDestination != Vector3.zero)
        {
            agent.destination = currentDestination; 
        }
    }
    
    public void SetDestination(Vector3 destination)
    {
        currentDestination = destination;
    }
}
