using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePickup : Pickup
{

    [SerializeField] private Note note;

    protected override bool destroy => false;

    protected override void OnPickedUp()
    {
        UIController.Instance.ShowNote(note);
    }

}