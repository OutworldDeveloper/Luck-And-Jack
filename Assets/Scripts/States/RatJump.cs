using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatJump : State
{

    public bool CanJump => rat.LookDirection != FlatVector.zero && !IsOnCooldown;
    public bool IsOnCooldown => Time.time < cooldownEndTime;
    public bool IsEnded => Time.time > endTime || hasSetDamage;

    private NPC_Rat rat;
    private NavMeshAgent agent;
    private CharacterController controller;
    private RotationController rotationController;
    private Animator animator;
    private SoundPlayer soundPlayer;

    private float endTime;
    private FlatVector jumpDirection;
    private float cooldownEndTime;
    private bool hasSetDamage;

    public RatJump(NPC_Rat rat, NavMeshAgent agent, CharacterController controller, RotationController rotationController, Animator animator, SoundPlayer soundPlayer)
    {
        this.rat = rat;
        this.agent = agent;
        this.controller = controller;
        this.rotationController = rotationController;
        this.animator = animator;
        this.soundPlayer = soundPlayer;
    }

    public override void OnEnter()
    {
        endTime = Time.time + 0.5f;
        cooldownEndTime = Time.time + Random.Range(4f, 13f);
        jumpDirection = rat.LookDirection;
        agent.isStopped = true;
        hasSetDamage = false;
        //agent.enabled = false;
        //animator.SetBool("IsDodging", true);
    }

    public override void OnExit()
    {
        agent.isStopped = false;
        //animator.SetBool("IsDodging", false);
        //agent.enabled = true;
    }

    public override void OnUpdate()
    {
        if (IsEnded) return;
        rotationController.LookIn(jumpDirection);
        controller.Move((jumpDirection.Vector3 * 12f + Vector3.down) * Time.deltaTime);

        if (FlatVector.Distance(rat.transform.position, Player.Luck.transform.position) > 1.3f)
            return;

        FlatVector targetDirection = Player.Luck.transform.position - rat.transform.position;
        float angle = FlatVector.Angle(targetDirection, rat.transform.forward);
        if (angle > 50f)
            return;

        Player.Luck.ApplyDamage(1, rat.transform.forward.FlatVector(), rat.Team);
        soundPlayer.PlaySound();

        hasSetDamage = true;
    }

}