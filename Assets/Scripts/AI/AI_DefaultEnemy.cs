using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(NPCVision))]
[RequireComponent(typeof(NPC_DefaultEnemy))]
public class AI_DefaultEnemy : StateMachineBehaviour
{

    private NPCVision vision;
    private NPC_DefaultEnemy npc_DefaultEnemy;

    private Chilling chilling;
    private Chasing chasing;
    private Attacking attacking;
    private Investigating investigating;

    protected override State initialState => chilling;

    private void Awake()
    {
        vision = GetComponent<NPCVision>();
        npc_DefaultEnemy = GetComponent<NPC_DefaultEnemy>();

        chilling = new Chilling(npc_DefaultEnemy);
        chasing = new Chasing(npc_DefaultEnemy);
        attacking = new Attacking(this, npc_DefaultEnemy);
        investigating = new Investigating(this, npc_DefaultEnemy);
    }

    protected override void SetupStateMachine()
    {
        stateMachine.AddTransition(chilling, chasing, () => vision.IsSeeingPlayer);

        stateMachine.AddTransition(chasing, investigating, () => !vision.IsSeeingPlayer);
        stateMachine.AddTransition(chasing, attacking, () => vision.DistanceToPlayer() < 3f);

        stateMachine.AddTransition(attacking, investigating, () => !vision.IsSeeingPlayer);
        stateMachine.AddTransition(attacking, chasing, () => vision.DistanceToPlayer() > 3f);

        stateMachine.AddTransition(investigating, chasing, () => vision.IsSeeingPlayer);
        stateMachine.AddTransition(investigating, chilling, () => investigating.IsOver);
    }

    protected override void Update()
    {
        base.Update();

        if (vision.IsSeeingPlayer)
        {
            investigating.SetInvestigationPosition(Player.Luck.transform.position);
        }
    }

}