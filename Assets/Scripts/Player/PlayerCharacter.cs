using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(RotationController))]
[RequireComponent(typeof(CharacterControllerAnimator))]
public abstract class PlayerCharacter : Actor
{

    [SerializeField] protected Animator animator;

    private CharacterControllerAnimator characterControllerAnimator;
    public FlatVector InputVector;

    protected abstract float speed { get; }

    protected RotationController rotationController;
    protected CharacterController controller;

    protected MovementState movementState;

    protected override State initialState => movementState;

    protected override void Awake()
    {
        base.Awake();
        characterControllerAnimator = GetComponent<CharacterControllerAnimator>();
        rotationController = GetComponent<RotationController>();
        controller = GetComponent<CharacterController>();
        movementState = new MovementState(this, rotationController, controller, characterControllerAnimator, animator, speed);
    }

}