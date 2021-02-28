using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
 
    [SerializeField] private Vector3 interactionPointOffset = Vector3.zero;
    [SerializeField] private Vector3 interactionRangeCenterPointOffset = Vector3.zero;
    [SerializeField] private Vector3 interactionFacingDirection = Vector3.forward;
    [SerializeField] private Vector3 interactionTextPosition = Vector3.up * 2f;
    [SerializeField] private float interactionRange = 2f;

    public event Action InteractionEndedCallback;

    public float InteractionRange => interactionRange;

    public FlatVector InteractionPoint =>
        transform.TransformPoint(interactionPointOffset);

    public FlatVector InteractionRangeCenterPoint => 
        transform.TransformPoint(interactionRangeCenterPointOffset);

    public FlatVector InteractionFacingDirection =>
          transform.TransformDirection(interactionFacingDirection);

    public Vector3 InteractionTextPosition => transform.position + interactionTextPosition;

    public abstract string InteractionText { get; }

    public abstract string AnimationTrigger { get; }

    public bool IsInteractionAvaliable()
    {
        if (isInteracting) 
            return false;
        return CanInteract();
    }

    protected abstract bool CanInteract();

    protected void StopInteraction()
    {
        isInteracting = false;
    }

    protected bool isInteracting { get; private set; }
    protected float interactionStartTime { get; private set; }

    public void Interact()
    {
        isInteracting = true;
        interactionStartTime = Time.time;
        OnInteracted();
    }

    protected abstract void OnInteracted();

    protected void EndInteraction()
    {
        isInteracting = false;
        InteractionEndedCallback?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(InteractionPoint, Vector3.up);
        Gizmos.DrawRay(InteractionPoint, InteractionFacingDirection);
        Gizmos.DrawWireSphere(InteractionRangeCenterPoint, interactionRange);
        Gizmos.DrawWireSphere(InteractionTextPosition, 0.2f);
    }

}