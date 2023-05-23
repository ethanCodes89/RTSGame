using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UniversalAssetsProject.Utilities;
public class BaseWorkerNetworkBehavior : BaseUnit
{
    public StateMachine WorkerStateMachine;

    private void Awake()
    {
        WorkerStateMachine.SetNextState(new IdleState());
    }
}
