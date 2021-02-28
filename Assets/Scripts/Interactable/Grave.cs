using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grave : Interactable
{

    public static event Action OnGraveSaved;

    [SerializeField] private Light graveLight = null;
    [SerializeField] private UnityEvent onSaved;  

    public override string InteractionText => "extract the soul";

    public bool IsSaved { get; private set; }
    public float LastSavedTime { get; private set; }

    public override string AnimationTrigger => "GraveSaving";

    public void Respawn()
    {
        IsSaved = false;
        graveLight.enabled = true;
        foreach (Transform item in transform)
            item.GetComponent<Candle>()?.EnableGlowing(true);
        GetComponent<Animator>().SetBool("Extractiion", false);
    }

    protected override bool CanInteract()
    {
        return !IsSaved;
    }

    protected override void OnInteracted()
    {
        GetComponent<Animator>().SetBool("Extractiion", true);
    }

    private void Update()
    {
        if (!isInteracting)
            return;

        if (Player.Luck.IsDead)
        {
            GetComponent<Animator>().SetBool("Extractiion", false);
            StopInteraction();
        }

        if (Time.time > interactionStartTime + 4.3f && !IsSaved)
        {
            graveLight.enabled = false;
            LastSavedTime = Time.time;
            IsSaved = true;
            OnGraveSaved?.Invoke();
            Player.Luck.ApplyHeal(1);
            foreach (Transform item in transform)
                item.GetComponent<Candle>()?.EnableGlowing(false);
            onSaved.Invoke();
        }

        if (Time.time > interactionStartTime + 8.45f)
            EndInteraction();
    }

}