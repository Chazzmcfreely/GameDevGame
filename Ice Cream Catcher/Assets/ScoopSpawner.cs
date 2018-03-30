using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoopSpawner : MonoBehaviour {

    public GameObject scoopPrefab;

    public float minDelay;
    public float maxDelay;

    float spawnDelay;
    float lastSpawnTime;

	// Use this for initialization
	void Start () {
        lastSpawnTime = 0;
        spawnDelay = Random.Range(minDelay, maxDelay);
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.time - lastSpawnTime > spawnDelay) {
            SpawnScoop();

            lastSpawnTime = Time.time;
            spawnDelay = Random.Range(minDelay, maxDelay);
            
        }
	}

    void SpawnScoop() {
        Vector2 spawnPosition = new Vector2(Random.Range(-9f, 9f), 40);
        Instantiate(scoopPrefab, spawnPosition, Quaternion.identity);
    }
}
