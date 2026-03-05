using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxSpawner : MonoBehaviour
{
    public GameObject hitboxPrefab;
    public GameObject hitcirclePrefab;
    public Transform hitboxSpawnPoint;

    public void SpawnHitbox()
    {
        Instantiate(hitboxPrefab, hitboxSpawnPoint.position, hitboxSpawnPoint.rotation);
    }
    public void SpawnHitcircle()
    {
        Instantiate(hitcirclePrefab, hitboxSpawnPoint.position, hitboxSpawnPoint.rotation);
    }
}
