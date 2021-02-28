using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Actor))]
[RequireComponent(typeof(BloodSpawner))]
public class CharacterBloodSpawner : MonoBehaviour
{

    [SerializeField] private ParticleSystem meleeBloodEffectPrefab;
    [SerializeField] private float minSplashDelay;
    [SerializeField] private float maxSplashDelay;

    private Actor actor;
    private BloodSpawner bloodSpawner;

    private void Awake()
    {
        actor = GetComponent<Actor>();
        bloodSpawner = GetComponent<BloodSpawner>();
    }

    private void Start()
    {
        actor.Damaged += OwnerDamageTaken;
    }

    private void OnDestroy()
    {
        actor.Damaged -= OwnerDamageTaken;
    }

    private void OwnerDamageTaken(int damage, int lastHealth, FlatVector direction)
    {
        // Particles
        Quaternion rotation = Quaternion.LookRotation(direction.Vector3, Vector3.up);
        Instantiate(meleeBloodEffectPrefab, transform.position + Vector3.up, rotation);

        // Decals 
        StartCoroutine(SpawnBloodWithDelay(direction, 10));
    }

    private IEnumerator SpawnBloodWithDelay(FlatVector damageDirection, int bloodAmount)
    {
        yield return new WaitForSeconds(Random.Range(minSplashDelay, maxSplashDelay));

        Vector3 origin = actor.transform.position + Vector3.up;
        int raysCount = bloodAmount;
        int successfulRays = raysCount;

        for (int a = 0; a < 3; a++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1f, -1f), Random.Range(-0.5f, 0.5f));
            Vector3 direction = damageDirection.Vector3 / 2 + offset;

            if (bloodSpawner.TrySpawnBlood(new Ray(origin, direction)))
            {
                successfulRays--;
                yield return new WaitForSeconds(0.005f);
            }
        }

        for (int a = 0; a < raysCount; a++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-1f, -0.25f), Random.Range(-0.25f, 0.25f));
            Vector3 direction = damageDirection.Vector3 + offset;

            if (bloodSpawner.TrySpawnBlood(new Ray(origin, direction)))
            {
                successfulRays--;
                yield return new WaitForSeconds(0.005f);
            }
        }

        for (int a = 0; a < successfulRays; a++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-1f, -1f), Random.Range(-0.5f, 0.5f));
            Vector3 direction = damageDirection.Vector3 + offset;

            if (bloodSpawner.TrySpawnBlood(new Ray(origin, direction)))
            {
                yield return new WaitForSeconds(0.005f);
            }
        }

    }

}