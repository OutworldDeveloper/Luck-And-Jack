using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackHatObject : MonoBehaviour
{

    [SerializeField] private new Renderer renderer;
    [SerializeField] private Material sleepingMaterial;
    [SerializeField] private Light ligth;

    private Material defaultMaterial;

    public void SetSleeping(bool sleep)
    {
        ligth.enabled = !sleep;
        renderer.material = sleep ? sleepingMaterial : defaultMaterial;
    }

    private void Awake()
    {
        defaultMaterial = renderer.material;
    }

}