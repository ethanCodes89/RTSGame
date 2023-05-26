using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UniversalAssetsProject.Utilities;
public class BaseWorkerNetworkBehavior : BaseSelectable
{
    public StateMachine WorkerStateMachine;

    private void Awake()
    {
        WorkerStateMachine.SetNextState(new IdleState());
    }
}
