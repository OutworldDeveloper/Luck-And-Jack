using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class NPC_DefaultEnemy : NPC, IKnockbackReciver
{

    [SerializeField] private Animator animator;
    [SerializeField] private SkinnedMeshRenderer targetRenderer;

    public bool IsAttacking;

    private CharacterController controller;

    private Trigger knockbackTrigger;

    private NPCMovementState movementState;
    private KatanaAttackDefault katanaAttackDefault;
    private KatanaAttackAlt katanaAttackAlt;
    private KnockbackState knockbackState;
    private KnockbackExitState knockbackExitState;

    protected override State initialState => movementState;

    protected override Team team => Team.Bad;

    public void SetKnockback(FlatVector direction, float force)
    {
        knockbackState.SetKnockback(direction, force, 0.3f);
        knockbackTrigger.SetActive();
    }

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<CharacterController>();

        knockbackTrigger = stateMachine.CreateTrigger();

        movementState = new NPCMovementState(this, agent, rotationController, animator);
        katanaAttackDefault = new KatanaAttackDefault(this, agent, controller, rotationController, animator);
        katanaAttackAlt = new KatanaAttackAlt(this, agent, controller, rotationController, animator);
        knockbackState = new KnockbackState(agent, controller, rotationController, animator);
        knockbackExitState = new KnockbackExitState(agent, animator);
    }

    protected override void SetupStateMachine()
    {
        stateMachine.AddTransition(movementState, katanaAttackDefault,
            () => IsAttacking && Random.Range(0, 2) == 0);

        stateMachine.AddTransition(movementState, katanaAttackAlt,
            () => IsAttacking);

        stateMachine.AddTransition(katanaAttackDefault, movementState, () => !katanaAttackDefault.IsAttacking);

        stateMachine.AddTransition(katanaAttackAlt, movementState, () => !katanaAttackAlt.IsAttacking);

        stateMachine.AddGlobalTransition(knockbackState, () => knockbackTrigger.IsActive);

        stateMachine.AddTransition(knockbackState, knockbackExitState, () => knockbackState.IsEnded);

        stateMachine.AddTransition(knockbackExitState, movementState, () => knockbackExitState.IsEnded);
    }

}