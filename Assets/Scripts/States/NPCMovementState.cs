using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovementState : State
{

    private Actor actor;
    private NavMeshAgent agent;
    private Animator animator;
    private RotationController rotationController;

    public NPCMovementState(Actor actor, NavMeshAgent agent, RotationController rotationController, Animator animator)
    {
        this.actor = actor;
        this.agent = agent;
        this.animator = animator;
        this.rotationController = rotationController;
    }

    public override void OnEnter()
    {
        rotationController.enabled = true;
        agent.enabled = true;
        agent.isStopped = false;
        agent.speed = 4f;
    }

    public override void OnExit()
    {
        rotationController.enabled = true;
    }

    public override void OnUpdate()
    {
        rotationController.LookAt(agent.steeringTarget.FlatVector());
    }

}