using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class NPC_Rat : NPC, IKnockbackReciver
{

    [SerializeField] private Animator animator;
    [SerializeField] private new Renderer renderer;
    [SerializeField] private Material deathMaterial;
    [SerializeField] private SoundPlayer biteSoundPlayer;

    public NPCMovementState MovementState { get; private set; }
    public RatJump JumpState { get; private set; }
    public RatDeathState DeathState { get; private set; }

    private Trigger jumpTrigger;

    private CharacterController controller;

    protected override State initialState => MovementState;
    protected override State GetDeathState() => DeathState;

    protected override Team team => Team.Bad;

    public void SetKnockback(FlatVector direction, float force)
    {
        DeathState.SetKnockback(direction, force);
    }

    public void Jump()
    {
        if (JumpState.CanJump)
            jumpTrigger.SetActive();
    }

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<CharacterController>();
        jumpTrigger = stateMachine.CreateTrigger();
        MovementState = new NPCMovementState(this, agent, rotationController, animator);
        JumpState = new RatJump(this, agent, controller, rotationController, animator, biteSoundPlayer);
        DeathState = new RatDeathState(controller, rotationController, agent, animator, renderer, deathMaterial);
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();
        stateMachine.AddTransition(MovementState, JumpState, () => jumpTrigger.IsActive);
        stateMachine.AddTransition(JumpState, MovementState, () => JumpState.IsEnded);
    }

}

public class RatDeathState : State
{

    private CharacterController controller;
    private RotationController rotationController;
    private NavMeshAgent agent;
    private Animator animator;
    private Renderer renderer;
    private Material deathMaterial;

    private FlatVector knockbackDirection;
    private float force;

    private float endTime;
    private float startToFadeOffTime;

    private bool isGoingToBeDestroyed;

    public RatDeathState(CharacterController controller, RotationController rotationController, 
        NavMeshAgent agent, Animator animator, Renderer renderer, Material deathMaterial)
    {
        this.controller = controller;
        this.rotationController = rotationController;
        this.agent = agent;
        this.animator = animator;
        this.renderer = renderer;
        this.deathMaterial = deathMaterial;
    }

    public void SetKnockback(FlatVector direction, float force)
    {
        knockbackDirection = direction.normalized;
        this.force = force;
    }

    public override void OnEnter()
    {
        controller.enabled = false;
        agent.enabled = true;
        agent.isStopped = true;
        rotationController.LookIn(-knockbackDirection.normalized);
        endTime = Time.time + 0.2f;
        startToFadeOffTime = Time.time + 10f;
        animator.SetTrigger("Death");
        renderer.sharedMaterial = deathMaterial;
    }

    public override void OnUpdate()
    {
        if (Time.time > endTime)
        {
            if (Time.time > startToFadeOffTime)
            {
                controller.transform.position = controller.transform.position + Vector3.down * Time.deltaTime;
                if (!isGoingToBeDestroyed)
                {
                    agent.enabled = false;
                    controller.enabled = false;
                    Object.Destroy(controller.gameObject, 2f);
                    isGoingToBeDestroyed = true;
                }
            }
            return;
        }
        rotationController.LookIn(-knockbackDirection.normalized);
        agent.Move(knockbackDirection.Vector3 * force * Time.deltaTime);
    }

}