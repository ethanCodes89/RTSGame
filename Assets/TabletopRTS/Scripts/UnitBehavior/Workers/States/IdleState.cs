using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    
    public override void OnEnter(StateMachine stateMachine)
    {
        base.OnEnter(stateMachine);
        //TODO: set idle animation
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //TODO: check if unit should transition to new state
    }

    public override void OnExit()
    {
        base.OnExit();
        //TODO: do anything needed before exiting idle state
    }
}
