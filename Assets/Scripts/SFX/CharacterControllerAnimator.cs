using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    private float localVelocityMagnitude;

    private void Update()
    {
        localVelocityMagnitude = Mathf.MoveTowards(localVelocityMagnitude, 
            characterController.velocity.magnitude, 20f * Time.deltaTime);
        animator.SetFloat("Speed", localVelocityMagnitude);
    }

}