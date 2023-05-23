using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnResourceState : State
{
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);
        //TODO: Send worker to base with resource
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //TODO: check if unit should transition to new state
    }

    public override void OnExit()
    {
        base.OnExit();
        //TODO: do anything needed before exiting return state
    }
}
