using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger
{ 

    public Trigger(StateMachine stateMachine)
    {
        stateMachine.StateChanged += () => IsActive = false;
    }

    public bool IsActive { get; private set; }

    public void SetActive() => IsActive = true;

}