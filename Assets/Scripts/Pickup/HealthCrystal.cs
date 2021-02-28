using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCrystal : Pickup
{
    protected override bool destroy => false;

    protected override void OnPickedUp()
    {
        Player.Luck.ApplyHeal(1);
    }

}