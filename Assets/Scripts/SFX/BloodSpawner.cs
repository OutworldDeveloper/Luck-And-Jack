using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BloodSpawner : MonoBehaviour
{

    [SerializeField] private DecalProjector[] bloodPrefabs;
    [SerializeField] private Vector2 minScale;
    [SerializeField] private Vector2 maxScale;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask layerMask;

    public bool TrySpawnBlood(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            Quaternion rotation = Quaternion.LookRotation(-hit.normal);
            DecalProjector decalProjector = Instantiate(GetRandomDecal(), hit.point + hit.normal * 0.1f, rotation);
            decalProjector.size = GetRandomSize();
            decalProjector.transform.eulerAngles = new Vector3(decalProjector.transform.eulerAngles.x, decalProjector.transform.eulerAngles.y, Random.Range(0, 360));
            return true;
        }
        return false;
    }

    private DecalProjector GetRandomDecal() => bloodPrefabs[Random.Range(0, bloodPrefabs.Length)];

    private Vector3 GetRandomSize() => new Vector3(Random.Range(minScale.x, maxScale.x), Random.Range(minScale.y, maxScale.y), 0.2f);

}