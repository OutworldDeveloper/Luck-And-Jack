using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckInteracting : State
{

    public bool IsInteracting { get; private set; }

    private Luck luck;
    private CharacterController controller;
    private RotationController rotationController;
    private Animator animator;

    private Interactable target;
    private bool startedInteraction;

    public LuckInteracting(Luck luck, CharacterController controller, RotationController rotationController, Animator animator)
    {
        this.luck = luck;
        this.controller = controller;
        this.rotationController = rotationController;
        this.animator = animator;
    }

    public bool FindInteractable()
    {
        foreach (var item in Object.FindObjectsOfType<Interactable>())
        {
            if (!item.IsInteractionAvaliable())
                continue;
            if (FlatVector.Distance(luck.transform.position, item.InteractionRangeCenterPoint) < item.InteractionRange)
            {
                target = item;
                return true;
            }
        }
        return false;
    }

    public override void OnEnter()
    {
        rotationController.LookIn(target.InteractionFacingDirection);
        target.InteractionEndedCallback += Target_InteractionEndedCallback;
        startedInteraction = false;
        IsInteracting = true;
    }

    public override void OnExit()
    {
        target.InteractionEndedCallback -= Target_InteractionEndedCallback;
    }

    public override void OnUpdate()
    {
        if (!startedInteraction)
        {
            if (FlatVector.Distance(luck.transform.position, target.InteractionPoint) > 0.01f)
            {
                luck.transform.position = Vector3.MoveTowards(luck.transform.FlatPosition(),
                target.InteractionPoint, 4.5f * Time.deltaTime);
                FlatVector direction = (target.InteractionPoint.Vector3 - luck.transform.position).normalized;
                rotationController.LookIn(direction);
                animator.SetFloat("Speed", 4.5f);
            }
            else
            {
                rotationController.LookIn(target.InteractionFacingDirection);
                luck.transform.position = target.InteractionPoint;
                target.Interact();
                startedInteraction = true;
                animator.SetTrigger(target.AnimationTrigger);
                animator.SetFloat("Speed", 0f);
            }
        }
    }

    private void Target_InteractionEndedCallback() => IsInteracting = false;

}