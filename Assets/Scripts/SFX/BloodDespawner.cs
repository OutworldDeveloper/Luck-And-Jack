using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(DecalProjector))]
public class BloodDespawner : MonoBehaviour
{

    [SerializeField] private float delay = 10f;

    private DecalProjector decalProjector;
    private float delayTime;
    private bool isGoingToBeDestroyed;

    private void Awake()
    {
        decalProjector = GetComponent<DecalProjector>();
    }

    private void Start()
    {
        delayTime = Time.time + delay;
    }

    private void Update()
    {
        if (Time.time < delayTime)
            return;
        decalProjector.fadeFactor -= Time.deltaTime;
        if (isGoingToBeDestroyed)
            return;
        Destroy(gameObject, 1.1f);
        isGoingToBeDestroyed = true;
    }

}