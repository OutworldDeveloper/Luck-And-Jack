using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chasing : State
{

    private NPC npc;

    public Chasing(NPC npc)
    {
        this.npc = npc;
    }

    public override void OnUpdate()
    {
        npc.MoveTo(Player.Luck.transform.FlatPosition());
    }

}