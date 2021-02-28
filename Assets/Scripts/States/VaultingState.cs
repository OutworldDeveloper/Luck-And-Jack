using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaultingState : State
{

    // Original speed is 5f
    private readonly float initialSpeed = 5f;
    private readonly float maxSpeed = 5f;
    private readonly float acceleration = 0f;

    public bool IsEnded;

    private PlayerCharacter player;
    private CharacterController controller;
    private RotationController rotationController;
    private Animator animator;

    private Vector3 lastStartPosition;
    private Vector3 lastEndPosition;
    private bool reachedStartPosition;
    private bool isVaulting;
    private float currentSpeed;

    public VaultingState(PlayerCharacter player, CharacterController controller, RotationController rotationController, Animator animator)
    {
        this.player = player;
        this.controller = controller;
        this.rotationController = rotationController;
        this.animator = animator;
    }

    public bool CanVault()
    {
        if (isVaulting)
            return false;

        Vector3 direction = player.InputVector.Vector3;

        float verticalOffset = 0.08f;
        float detectionRange = 2f;
        float playerSize = 0.25f;
        Vector3 startPosition = player.transform.position + Vector3.up * verticalOffset;

        if (direction == Vector3.zero) 
            return false;

        // First we should check if there is an obstacle in our way.

        RaycastHit hit;
        Ray ray = new Ray(startPosition, direction);
        if (!Physics.Raycast(ray, out hit, detectionRange))
            return false;

        //Debug.DrawLine(startPosition, hit.point, Color.red, 5f);

        Vector3 vaultStartPosition = hit.point;
        vaultStartPosition = hit.point - direction * playerSize;

        //Debug.DrawRay(vaultStartPosition, Vector3.up * 2f, Color.red, 5f);

        // Now we should check if the obstacle can be vaulted

        Vector3 endPosition = default;
        bool idk = false;

        for (int i = 0; i < 6; i++)
        {
            // This might not work
            if (i < 6)
            {
                if (Physics.Linecast(vaultStartPosition + Vector3.up + direction * i * 0.5f,
                    vaultStartPosition + Vector3.up + direction * (i + 1) * 0.5f))
                    return false;
            }

            RaycastHit currentHit;
            Ray currentRay = new Ray(vaultStartPosition + Vector3.up + direction * (1f + i ) * 0.5f, Vector3.down);
            if (Physics.Raycast(currentRay, out currentHit, 1f - verticalOffset))
            {
                Debug.DrawRay(currentRay.origin, currentRay.direction, Color.red, 5f);
            }
            else
            {
                Debug.DrawRay(currentRay.origin, currentRay.direction, Color.green, 5f);
                //endPosition = currentRay.origin + currentRay.direction * (1f - verticalOffset);
                endPosition = currentRay.origin;
                idk = true;
                break;
            }
        }

        if (!idk)
            return false;

        Debug.DrawLine(endPosition, vaultStartPosition + Vector3.up, Color.magenta, 5f);
        if (Physics.Linecast(vaultStartPosition + Vector3.up, endPosition))
            return false;

        lastStartPosition = vaultStartPosition;
        lastStartPosition.y = 0f;
        lastEndPosition = endPosition + Vector3.down * (1f - verticalOffset);
        lastEndPosition.y = 0f;
        return true;
    }

    public override void OnEnter()
    {
        isVaulting = true;
        IsEnded = false;
        reachedStartPosition = false;
        //controller.enabled = false;
        rotationController.LookIn((lastEndPosition - lastStartPosition).normalized.FlatVector());
        currentSpeed = initialSpeed;
    }

    public override void OnExit()
    {
        isVaulting = false;
        controller.enabled = true;
        animator.SetBool("IsVaulting", false);
    }

    public override void OnUpdate()
    {
        if (IsEnded) return;
        if (!reachedStartPosition)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, lastStartPosition, 10f * Time.deltaTime);
            if (Vector3.Distance(player.transform.position, lastStartPosition) < 0.01f)
            {
                animator.SetBool("IsVaulting", true);
                reachedStartPosition = true;
            }
            return;
        }
        player.transform.position = Vector3.MoveTowards(player.transform.position, lastEndPosition, currentSpeed * Time.deltaTime);
        currentSpeed = Mathf.Min(maxSpeed, currentSpeed + acceleration * Time.deltaTime);
        if(Vector3.Distance(player.transform.position, lastEndPosition) < 0.01f)
        {
            player.transform.position = lastEndPosition;
            IsEnded = true;
        }
        //IsEnded = player.transform.position == lastEndPosition;
    }

}