using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(RotationController))]
public abstract class NPC : Actor
{

    protected NavMeshAgent agent;
    protected RotationController rotationController;

    public virtual void MoveTo(FlatVector location)
    {
        if (IsDead)
            return;
        agent.SetDestination(location.Vector3);
    }

    public virtual void Stop()
    {
        agent.ResetPath();
    }

    protected override  void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        rotationController = GetComponent<RotationController>();
        agent.updateRotation = false;
    }

    protected override void OnDeath()
    {
        Stop();
    }

}