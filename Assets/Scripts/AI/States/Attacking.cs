using UnityEngine;
using UnityEngine.AI;

public class Attacking : State
{

    private AI_DefaultEnemy ai;
    private NPC_DefaultEnemy npc;

    public Attacking(AI_DefaultEnemy ai, NPC_DefaultEnemy npc)
    {
        this.ai = ai;
        this.npc = npc;
    }

    public override void OnEnter()
    {
        npc.Stop();
    }

    public override void OnExit()
    {
        npc.IsAttacking = false;
    }

    public override void OnUpdate()
    {
        npc.SetLookDirection((Player.Luck.transform.position - npc.transform.position).normalized);
        npc.IsAttacking = true;
    }

}