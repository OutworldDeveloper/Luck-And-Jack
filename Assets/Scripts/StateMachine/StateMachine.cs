using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{

    public Action StateChanged;
    public State CurrentState { get; private set; }

    private Dictionary<State, List<Transition>> transitions = new Dictionary<State, List<Transition>>();
    private List<Transition> globalTransitions = new List<Transition>();

    public Trigger CreateTrigger() => new Trigger(this);

    public void Tick()
    {
        if (FindTransition(out var transition))
            SetState(transition.To);
        CurrentState.OnUpdate();
    }

    public void FixedUpdateTick()
    {
        CurrentState.OnFixedUpdate();
    }

    public void SetState(State state)
    {
        // It works but it is not a good solution 
        StateChanged?.Invoke();

        if (state == CurrentState) return;
        CurrentState?.OnExit();
        CurrentState = state;
        CurrentState.OnEnter();

        // Should be here, but doesn't work as expected (Triggers problem)
        //StateChanged?.Invoke();
    }

    public void AddTransition(State from, State to, Func<bool> condition)
    {
        if (!transitions.TryGetValue(from, out var _transitions))
        {
            _transitions = new List<Transition>();
            transitions[from] = _transitions;
        }
        _transitions.Add(new Transition(to, condition));
    }

    public void AddGlobalTransition(State to, Func<bool> condition)
    {
        globalTransitions.Add(new Transition(to, condition));
    }

    private bool FindTransition(out Transition transition)
    {
        foreach (var item in globalTransitions)
        {
            if (item.ShouldTransist())
            {
                transition = item;
                return true;
            }
        }

        if (transitions.ContainsKey(CurrentState))
        {
            foreach (var item in transitions[CurrentState])
            {
                if (item.ShouldTransist())
                {
                    transition = item;
                    return true;
                }
            }
        }

        transition = default;
        return false;
    }

}