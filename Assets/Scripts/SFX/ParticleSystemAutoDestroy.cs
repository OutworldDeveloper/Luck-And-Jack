using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemAutoDestroy : MonoBehaviour
{

    private new ParticleSystem particleSystem;

    private void Awake() => particleSystem = GetComponent<ParticleSystem>();

    private void Update()
    {
        if (particleSystem.IsAlive(true)) 
            return;
        Destroy(gameObject);
    }

}