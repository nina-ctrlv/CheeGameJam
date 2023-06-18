using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> objectsInRange;

    public Card spawnData;

    public GameObject spawnObject;

    void Start()
    {
        this.GetComponent<CircleCollider2D>().radius = spawnData.spawnRange;
        InvokeRepeating("spawn", spawnData.spawnFrequency, spawnData.spawnFrequency);
    }

    void spawn()
    {
        if (/*foodSpawner ||*/ objectsInRange.Count > 0)
        {
            // Get location to spawn at
            Vector2 spawnLocation = transform.position;
            var newObj = Instantiate(spawnObject, spawnLocation + new Vector2(1, 0), Quaternion.identity);

            // apply all effects to spawned item

            // newObj.getComponent<projectile || food>().speed = spawnData.spawnedItemSpeed
            // newObj.getComponent<projectile || food>().cookLevel = spawnData.cookLevel
            // newObj.GetComponent<SpriteRenderer>().sprite = spawnData.spawnedArtwork;
            // newObj.GetComponent<testSpawnedItem>().speed = spawnData.spawnedItemSpeed;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        objectsInRange.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        objectsInRange.Remove(other.gameObject);
    }
}
