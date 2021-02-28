using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MeleeAttack : State
{

    public bool IsAttacking { get; protected set; }

    private Actor actor;
    private NavMeshAgent agent;
    private CharacterController controller;
    private RotationController rotationController;
    private Animator animator;
    private bool hasSetDamage;

    protected float attackStartTime;
    protected float attackEndTime;
    protected FlatVector attackDirection;

    protected abstract float attackTime { get; }
    protected abstract float damageDelay { get; }
    protected abstract string animationTriggerName { get; }

    public MeleeAttack(Actor actor, CharacterController controller, RotationController rotationController, Animator animator)
    {
        this.actor = actor;
        this.controller = controller;
        this.rotationController = rotationController;
        this.animator = animator;
    }

    public MeleeAttack(Actor actor, NavMeshAgent agent, CharacterController controller, RotationController rotationController, Animator animator)
        : this(actor, controller, rotationController, animator)
    {
        this.agent = agent;
    }

    public override void OnEnter()
    {
        hasSetDamage = false;
        IsAttacking = true;
        attackStartTime = Time.time;
        attackEndTime = Time.time + attackTime;
        animator.SetTrigger(animationTriggerName);
        attackDirection = actor.LookDirection;
        if (agent)
        {
            //agent.speed = 0;
            agent.isStopped = true;
            agent.enabled = false;
        }
    }

    public override void OnExit()
    {
        if (agent)
        {
            //agent.speed = 3.5f;
            agent.isStopped = false;
            agent.enabled = true;
        }
    }

    public override void OnUpdate()
    {
        if (!IsAttacking) return;
        if (Time.time < attackEndTime)
        {
            rotationController.LookIn(attackDirection);
            controller.Move(attackDirection.Vector3 * 4f * Time.deltaTime);
            if (!hasSetDamage && Time.time > attackStartTime + damageDelay)
            {
                hasSetDamage = true;
                SetDamage();
            }
            Debug.DrawLine(actor.transform.position, actor.transform.position + Quaternion.AngleAxis(-60f, Vector3.up) * actor.transform.forward * 1.5f);
            Debug.DrawLine(actor.transform.position, actor.transform.position + Quaternion.AngleAxis(60f, Vector3.up) * actor.transform.forward * 1.5f);
        }
        else
            IsAttacking = false;
    }

    private void SetDamage()
    {
        Collider[] hits = Physics.OverlapSphere(actor.transform.position, 1.5f);
        foreach (var item in hits)
        {
            if (item.gameObject == actor.gameObject)
                continue;

            if (Vector3.Distance(actor.transform.position, item.transform.position) > 0.35f)
            {
                Vector3 targetDir = item.transform.position - actor.transform.position;
                float angle = Vector3.Angle(targetDir, actor.transform.forward);
                if (angle > 60f)
                    continue;
            }

            //item.GetComponent<IKnockbackReciver>()?.SetKnockback(attackDirection, 7f);
        }
    }

}

public class KatanaAttackDefault : MeleeAttack
{

    public KatanaAttackDefault(Actor actor, NavMeshAgent agent, CharacterController controller, RotationController rotationController, Animator animator) 
        : base(actor, agent, controller, rotationController, animator) { }

    public KatanaAttackDefault(Actor actor, CharacterController controller, RotationController rotationController, Animator animator) 
        : base(actor, controller, rotationController, animator) { }

    protected override float attackTime => 0.25f;

    protected override float damageDelay => 0.1f;

    protected override string animationTriggerName => "Attack";

}

public class KatanaAttackAlt : MeleeAttack
{

    public KatanaAttackAlt(Actor actor, NavMeshAgent agent, CharacterController controller, RotationController rotationController, Animator animator)
        : base(actor, agent, controller, rotationController, animator) { }

    public KatanaAttackAlt(Actor actor, CharacterController controller, RotationController rotationController, Animator animator)
        : base(actor, controller, rotationController, animator) { }

    protected override float attackTime => 0.25f;

    protected override float damageDelay => 0.1f;

    protected override string animationTriggerName => "AttackAlt";

}