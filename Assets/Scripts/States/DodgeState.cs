using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : State
{

    public bool CanDodge => Time.time > dodgeAvaliableTime && player.InputVector != FlatVector.zero;
    public bool IsEnded => Time.time > endTime || isEnded;

    private PlayerCharacter player;
    private CharacterController controller;
    private RotationController rotationController;
    private Animator animator;

    private float endTime;
    private FlatVector dodgeDirection;
    private bool isEnded;
    private float dodgeAvaliableTime;

    public DodgeState(PlayerCharacter player, CharacterController controller, RotationController rotationController, Animator animator)
    {
        this.player = player;
        this.controller = controller;
        this.rotationController = rotationController;
        this.animator = animator;
    }

    public override void OnEnter()
    {
        endTime = Time.time + 0.45f;
        dodgeDirection = player.InputVector;
        //animator.SetTrigger("Dodge");
        animator.SetBool("IsDodging", true);
        isEnded = false;
    }

    public override void OnExit()
    {
        //animator.SetTrigger("DodgeEnd");
        animator.SetBool("IsDodging", false);
        dodgeAvaliableTime = Time.time + 0.4f;
    }

    public override void OnUpdate()
    {
        if (IsEnded) return;
        rotationController.LookIn(dodgeDirection);
        controller.Move(dodgeDirection.Vector3 * 10f * Time.deltaTime);
        //isEnded = controller.velocity.magnitude < 0.001f;
    }

}