using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class StateMachineBehaviour : MonoBehaviour
{

    [SerializeField] private bool debugState;

    protected StateMachine stateMachine = new StateMachine();
    protected abstract State initialState { get; }

    protected virtual void Start()
    {
        SetupStateMachine();
        stateMachine.SetState(initialState);
    }

    protected virtual void Update()
    {
        stateMachine.Tick();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.FixedUpdateTick();
    }

    protected abstract void SetupStateMachine();


#if UNITY_EDITOR

    protected virtual void OnDrawGizmos()
    {
        if (!debugState) return;
        if (!Application.isPlaying) return;
        if (stateMachine == null) return;
        if (stateMachine.CurrentState == null) return;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 25;

        Handles.Label(transform.position, stateMachine.CurrentState.ToString(), style);
    }

#endif

}