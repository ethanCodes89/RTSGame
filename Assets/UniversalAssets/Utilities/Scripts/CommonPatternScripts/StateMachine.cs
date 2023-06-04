using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public string CustomName;

    public State CurrentState { get; private set; }
    private State nextState;
    [SerializeField]
    [SerializeReference]
    private State mainStateType;

    private void Awake()
    {
        SetNextStateToMain();
    }

    void Update()
    {
        if(nextState != null)
        {
            SetState(nextState);
        }

        if(CurrentState != null)
        {
            CurrentState.OnUpdate();
        }
    }

    private void SetState(State newState)
    {
        nextState = null;
        if(CurrentState != null)
        {
            CurrentState.OnExit();
        }

        CurrentState = newState;
        CurrentState.OnEnter(this);
    }

    public void SetNextState(State newState)
    {
        if(newState != null)
        {
            nextState = newState;
        }
    }

    private void LateUpdate()
    {
        if(CurrentState != null)
        {
            CurrentState.OnLateUpdate();
        }
    }

    private void FixedUpdate()
    {
        if(CurrentState != null)
        {
            CurrentState.OnFixedUpdate();
        }
    }

    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }
}
