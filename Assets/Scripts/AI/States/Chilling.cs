using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chilling : State
{

    private NPC npc;

    public Chilling(NPC npc)
    {
        this.npc = npc;
    }

    public override void OnEnter()
    {
        npc.Stop();
    }

}