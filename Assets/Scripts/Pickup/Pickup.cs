using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{

    [SerializeField] private float pickupRadius = 1f;

    private bool playerInRadius;

    protected abstract bool destroy { get; }

    protected abstract void OnPickedUp();

    protected virtual void Update()
    {
        if (Player.Luck.IsDead)
            return;

        float distance = FlatVector.Distance(Player.Luck.transform.position, transform.position);

        if (distance < pickupRadius)
        {
            if (!playerInRadius)
            {
                OnPickedUp();
                if (destroy)
                    Destroy(gameObject);
                playerInRadius = true;
            }
        }
        else
        {
            if (distance > pickupRadius + 2f)
                playerInRadius = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }

}