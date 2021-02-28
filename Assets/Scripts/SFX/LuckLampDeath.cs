using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class LuckLampDeath : MonoBehaviour
{

    private new Rigidbody rigidbody;
    private new Collider collider;

    private float fallingTime;
    private bool hasFell;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void Start()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        Player.Luck.Died += Luck_Died;
    }

    private void OnDestroy()
    {
        Player.Luck.Died -= Luck_Died;
    }

    private void Luck_Died()
    {
        fallingTime = Time.time + 0f;
    }

    private void Update()
    {
        if (!Player.Luck.IsDead)
            return;
        if (hasFell)
            return;
        if (Time.time < fallingTime)
            return;

        hasFell = true;
        rigidbody.isKinematic = false;
        collider.enabled = true;
        transform.parent = null;
    }

}