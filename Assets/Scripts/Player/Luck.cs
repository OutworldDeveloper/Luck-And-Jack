using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luck : PlayerCharacter
{

    private Trigger dodgeTrigger;
    private Trigger vaultTrigger;
    private Trigger interactionTrigger;

    private DodgeState dodgeState;
    private VaultingState vaultingState;
    private LuckInteracting interactingState;

    protected override float speed => 4.5f;
    protected override Team team => Team.Good;

    public void TryDodge()
    {
        if (dodgeState.CanDodge)
            dodgeTrigger.SetActive();
    }

    public void TryVault()
    {
        if (vaultingState.CanVault())
            vaultTrigger.SetActive();
    }

    public void TryInteract()
    {
        if (interactingState.FindInteractable())
            interactionTrigger.SetActive();
    }

    protected override void Awake()
    {
        base.Awake();
        dodgeTrigger = stateMachine.CreateTrigger();
        vaultTrigger = stateMachine.CreateTrigger();
        interactionTrigger = stateMachine.CreateTrigger();
        dodgeState = new DodgeState(this, controller, rotationController, animator);
        vaultingState = new VaultingState(this, controller, rotationController, animator);
        interactingState = new LuckInteracting(this, controller, rotationController, animator);
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        // movementState
        stateMachine.AddTransition(movementState, vaultingState, () => vaultTrigger.IsActive);
        stateMachine.AddTransition(movementState, dodgeState, () => dodgeTrigger.IsActive);
        stateMachine.AddTransition(movementState, interactingState, () => interactionTrigger.IsActive);

        // dodgeState
        stateMachine.AddTransition(dodgeState, movementState, () => dodgeState.IsEnded);

        // vaultingState
        stateMachine.AddTransition(vaultingState, movementState, () => vaultingState.IsEnded);

        // interactingState
        stateMachine.AddTransition(interactingState, movementState, () => !interactingState.IsInteracting);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        animator.SetBool("Dead", true);
    }

}