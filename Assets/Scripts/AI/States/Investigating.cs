using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Investigating : State
{

    public event Action OnIvestigatingOver;

    public bool IsOver { get; private set; }

    private AI_DefaultEnemy ai;
    private NPC npc;
    //private NavMeshAgent agent;
    private Vector3 investigationPosition;

    public Investigating(AI_DefaultEnemy ai, NPC npc)
    {
        this.ai = ai;
        this.npc = npc;
        //this.agent = agent;
    }

    public void SetInvestigationPosition(Vector3 position)
    {
        investigationPosition = position;
    }

    public override void OnEnter()
    {
        //agent.ResetPath();
        //agent.stoppingDistance = UnityEngine.Random.Range(0.25f, 1f);
        IsOver = false;
        //agent.SetDestination(investigationPosition);
        npc.MoveTo(investigationPosition.FlatVector());
    }

    public override void OnExit()
    {
        //agent.stoppingDistance = 0;
        //agent.ResetPath();
        npc.Stop();
    }

    public override void OnUpdate()
    {
        //npc.GetComponent<RotationController>().LookAtIgnoringY(agent.steeringTarget);

        if (IsOver) return;

        //float distanceTarget = Vector3.Distance(npc.transform.position, agent.destination);
        //if (distanceTarget <= agent.stoppingDistance)
        //{
        //    IsOver = true;
        //    OnIvestigatingOver?.Invoke();
        //}
    }

}