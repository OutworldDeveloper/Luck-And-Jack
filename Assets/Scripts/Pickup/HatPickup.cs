using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatPickup : Pickup
{

    [SerializeField] private JackHat hat;

    protected override bool destroy => true;

    private void Start()
    {
        if (hat.IsUnlocked())
            Destroy(gameObject);
        else
            Instantiate(hat.HatPrefab, transform).SetSleeping(true);
    }

    protected override void OnPickedUp()
    {
        UIController.Instance?.ShowHelpText(hat.DisplayName + " unlocked!", 3.5f);
        hat.Unlock();
        if (FlatVector.Distance(transform.position, Player.Jack.transform.position) < 6f)
            PlayerProfile.Equip(hat);
    }

}