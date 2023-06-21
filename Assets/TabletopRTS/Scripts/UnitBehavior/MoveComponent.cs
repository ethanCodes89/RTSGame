using System;
using TabletopRTS.Scripts.UnitBehavior;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    private float speed;
    private Vector3 currentDestination;

    private void Start()
    {
        var unit = GetComponent<IUnit>();
        speed = unit.Speed;
        currentDestination = Vector3.zero;
    }

    private void Update() //TODO: once path finding is implemented, this will need reworked
    {
        if (currentDestination != Vector3.zero)
        {
            Vector3 direction = currentDestination - gameObject.transform.position;
            direction.y = 0f;

            float movementAmount = speed * Time.deltaTime;

            if (direction.magnitude <= movementAmount)
            {
                gameObject.transform.position = currentDestination;
                currentDestination = Vector3.zero;
            }
            else
            {
                gameObject.transform.position += direction.normalized * movementAmount;
            }   
        }
    }
    
    public void SetDestination(Vector3 destination)
    {
        currentDestination = destination;
    }
}
