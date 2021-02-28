using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackSaveInteractable : Interactable
{

    [SerializeField] private Light light;

    public override string InteractionText => "wake up Jack";
    public override string AnimationTrigger => "JackSaving";

    private bool jackSaved;

    protected override bool CanInteract() => !jackSaved;

    protected override void OnInteracted()
    {
        
    }

    private void Update()
    {
        if (jackSaved)
            return;
        if (!isInteracting)
            return;
        if (Time.time < interactionStartTime + 4.96f)
            return;
        (BaseGamemode.Instance as Gamemode_Tutorial)?.JackSaved();
        EndInteraction();
        jackSaved = true;
        light.enabled = false;
        foreach (Transform transform in transform)
            transform.GetComponent<Candle>()?.EnableGlowing(false);
    }

}