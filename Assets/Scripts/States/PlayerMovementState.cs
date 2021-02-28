using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : State
{

    private PlayerCharacter playerController;
    private RotationController rotationController;
    private CharacterController controller;
    private CharacterControllerAnimator characterControllerAnimator;
    private Animator animator;
    private float speed;

    private float afkTime;
    private float idleAnimationCooldownTime;

    public MovementState(PlayerCharacter playerController, RotationController rotationController, CharacterController controller, CharacterControllerAnimator characterControllerAnimator, Animator animator, float speed)
    {
        this.playerController = playerController;
        this.rotationController = rotationController;
        this.controller = controller;
        this.characterControllerAnimator = characterControllerAnimator;
        this.speed = speed;
        this.animator = animator;
    }

    public override void OnEnter()
    {
        characterControllerAnimator.enabled = true;
    }

    public override void OnExit()
    {
        characterControllerAnimator.enabled = false;
    }

    public override void OnUpdate()
    {
        rotationController.LookIn(playerController.InputVector);
        controller.Move((playerController.InputVector.Vector3 * speed + Physics.gravity) * Time.deltaTime);
        if (playerController.InputVector == FlatVector.zero)
        {
            afkTime += Time.deltaTime;
            if (afkTime > 3f && Time.time > idleAnimationCooldownTime)
            {
                animator.SetTrigger("idle");
                idleAnimationCooldownTime = Time.time + Random.Range(4f, 20f);
                return;
            }
        }
        else
        {
            afkTime = 0f;
            idleAnimationCooldownTime = Time.time + Random.Range(4f, 20f);
        }
    }

}