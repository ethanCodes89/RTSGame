using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected float time { get; set; }
    protected float fixedTime { get; set; }
    protected float lateTime { get; set; }
    public StateMachine StateMachineRef;

    public virtual void OnEnter(StateMachine stateMachine)
    {
        StateMachineRef = stateMachine;
    }

    public virtual void OnUpdate()
    {
        time += Time.deltaTime;
    }

    public virtual void OnFixedUpdate()
    {
        fixedTime += Time.deltaTime;
    }

    public virtual void OnLateUpdate()
    {
        lateTime += Time.deltaTime;
    }

    public virtual void OnExit()
    {

    }

    #region Passthrough Functions

    protected static void Destroy(UnityEngine.Object obj) //Removes a gameobject, component, or asset
    {
        UnityEngine.Object.Destroy(obj);
    }

    protected T GetComponent<T>() where T : Component { return StateMachineRef.GetComponent<T>(); } //Returns the component of type T if the gameobject has one attached, otherwise returns null

    protected Component GetComponent(System.Type type) { return StateMachineRef.GetComponent(type); } //Returns the component of Type (type) if the gameobject has one attached, otherwise returns null

    protected Component GetComponent(string type) { return StateMachineRef.GetComponent(type); } //Returns the component of Type ("type") if the gameobject has one attached, otherwise returns null
    #endregion
}
