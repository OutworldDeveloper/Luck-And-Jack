using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class NPCVision : MonoBehaviour
{

    [Header("Vision Raycast")]
    [SerializeField] private LayerMask detectionMask;
    [Header("Settings")]
    [SerializeField] private float visionAngle;
    [SerializeField] private float visionRange;
    [SerializeField] private float absoluteVisionRange;
    [SerializeField] private float reactionTime = 0.35f;

    public bool IsSeeingPlayer => isPlayerNoticed;

    private Actor actor;
    private bool isPlayerNoticed;
    private float timeSeeingPlayer;

    public float DistanceToPlayer()
    {
        return FlatVector.Distance(transform.FlatPosition(), Player.Luck.transform.FlatPosition());
    }

    public float AngleToPlayer()
    {
        FlatVector direction = Player.Luck.transform.FlatPosition() - transform.FlatPosition();
        return FlatVector.Angle(direction, transform.forward);
    }

    private void Awake()
    {
        actor = GetComponent<Actor>();
    }

    private void Update()
    {
        if (IsPlayerInVision())
        {
            timeSeeingPlayer += Time.deltaTime;
            if (timeSeeingPlayer >= reactionTime)
            {
                if (!isPlayerNoticed)
                {
                    // Event
                }
                isPlayerNoticed = true;
            }
        }
        else
        {
            if (isPlayerNoticed)
            {
                // Event
            }
            timeSeeingPlayer = 0f;
            isPlayerNoticed = false;
        }
    }

    private bool IsPlayerInVision()
    {
        PlayerCharacter player = Player.Luck;
        float playerDistance = DistanceToPlayer();

        if (playerDistance > visionRange) return false;

        if (playerDistance > absoluteVisionRange)
        {
            if (AngleToPlayer() > visionAngle) return false;
        }

        return !Physics.Linecast(actor.EyesPosition, player.EyesPosition, detectionMask);
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        //Handles.DrawWireDisc(this.transform.position, this.transform.up, attackRange);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(this.transform.position, this.transform.up, visionRange);
        Handles.DrawWireDisc(this.transform.position, this.transform.up, absoluteVisionRange);
        Handles.color = Color.green;
        //Handles.DrawWireDisc(this.transform.position, this.transform.up, targetLostRange);
        Actor npc = GetComponent<Actor>();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(npc.transform.position, npc.transform.position + Quaternion.AngleAxis(-visionAngle, Vector3.up) * npc.transform.forward * visionRange);
        Gizmos.DrawLine(npc.transform.position, npc.transform.position + Quaternion.AngleAxis(visionAngle, Vector3.up) * npc.transform.forward * visionRange);
    }

#endif

}