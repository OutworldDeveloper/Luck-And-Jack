using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float animationSpeedMultiplier = 0.5f;

    private Vector3 localMove;

    private void Update()
    {
        localMove = Vector3.Lerp(localMove, transform.InverseTransformDirection(agent.velocity), 25f * Time.deltaTime);
        animator.SetFloat("Horizontal", localMove.x);
        animator.SetFloat("Vertical", localMove.z);
        animator.SetFloat("Speed", agent.velocity.magnitude * animationSpeedMultiplier);
    }

}