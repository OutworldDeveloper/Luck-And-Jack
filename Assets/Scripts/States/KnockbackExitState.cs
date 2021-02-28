using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Можно сделать анимацию вставания из кнокнутого состояния
public class KnockbackExitState : State
{

    public bool IsEnded => Time.time > endTime;

    private float endTime;
    private NavMeshAgent agent;
    private Animator animator;

    public KnockbackExitState(Animator animator)
    {
        this.animator = animator;
    }

    public KnockbackExitState(NavMeshAgent agent, Animator animator)
        : this(animator)
    {
        this.agent = agent;
    }

    public override void OnEnter()
    {
        if (agent)
            agent.enabled = false;
        endTime = Time.time + 0.55f;
        animator.SetTrigger("StunnedEnded");
    }

    public override void OnExit()
    {
        if (agent)
            agent.enabled = true;
    }

}