using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Можно сделать анимацию вставания из кнокнутого состояния
public class KnockbackState : State
{

    public bool IsEnded => Time.time > endTime;

    private float endTime;
    private NavMeshAgent agent;
    private CharacterController controller;
    private RotationController rotationController;
    private Animator animator;

    private FlatVector knockbackDirection;
    private float force;

    public KnockbackState(CharacterController controller, RotationController rotationController, Animator animator)
    {
        this.controller = controller;
        this.rotationController = rotationController;
        this.animator = animator;
    }

    public KnockbackState(NavMeshAgent agent, CharacterController controller, RotationController rotationController, Animator animator)
        : this(controller, rotationController, animator)
    {
        this.agent = agent;
    }

    public void SetKnockback(FlatVector direction, float force, float duration)
    {
        knockbackDirection = direction.normalized;
        this.force = force;
        endTime = Time.time + duration;
    }

    public override void OnEnter()
    {
        if (agent)
            agent.enabled = false;
        animator.SetTrigger("StunnedAlt");
        rotationController.enabled = true;
    }

    public override void OnExit()
    {
        if (agent)
            agent.enabled = true;
        //animator.SetTrigger("StunnedEnded");
    }

    public override void OnUpdate()
    {
        if (IsEnded) return;
        rotationController.LookIn(-knockbackDirection.normalized);
        //KnockbackForce -= new Vector3(9.8f, 0f, 9.8f);
        controller.Move(knockbackDirection.Vector3 * force * Time.deltaTime);
    }

}