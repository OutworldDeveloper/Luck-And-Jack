using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NPC_Rat))]
[RequireComponent(typeof(NPCVision))]
public class AI_Rat : StateMachineBehaviour
{

    private NPCVision vision;
    private NPC_Rat npc_rat;
    private AIState_RatAttacking ratAttacking;
    private AIState_RunningAway runningAway;

    protected override State initialState => ratAttacking;

    private void Awake()
    {
        vision = GetComponent<NPCVision>();
        npc_rat = GetComponent<NPC_Rat>();
        ratAttacking = new AIState_RatAttacking(npc_rat, vision);
        runningAway = new AIState_RunningAway(npc_rat);
    }

    protected override void Start()
    {
        base.Start();
        npc_rat.Died += () => Destroy(this);
    }

    protected override void SetupStateMachine()
    {
        stateMachine.AddTransition(ratAttacking, runningAway, 
            () => npc_rat.JumpState.IsOnCooldown || Player.Luck.IsDead);
        stateMachine.AddTransition(runningAway, ratAttacking,
            () => !npc_rat.JumpState.IsOnCooldown && !Player.Luck.IsDead);
    }

}

public class AIState_RatAttacking : State
{

    private NPC_Rat rat;
    private NPCVision vision;

    public AIState_RatAttacking(NPC_Rat rat, NPCVision vision)
    {
        this.rat = rat;
        this.vision = vision;
    }

    public override void OnUpdate()
    {
        rat.MoveTo(Player.Luck.transform.FlatPosition());
        rat.SetLookDirection((Player.Luck.transform.FlatPosition() - rat.transform.FlatPosition()).normalized);
        if (!vision.IsSeeingPlayer)
            return;
        if (FlatVector.Distance(Player.Luck.transform.FlatPosition(), rat.transform.FlatPosition()) < 5f)
            rat.Jump();
    }

}

public class AIState_RunningAway : State
{

    private NPC npc;

    public AIState_RunningAway(NPC npc)
    {
        this.npc = npc;
    }

    public override void OnEnter()
    {
        npc.MoveTo(BaseGamemode.Instance.GetClosestRatSpawnPoint(npc.transform.position).transform.position);
    }

}