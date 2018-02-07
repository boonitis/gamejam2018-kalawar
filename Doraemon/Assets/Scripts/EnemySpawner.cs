using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject EnemyGO;

    public float maxSpawnRateInSeconds;
    public float minSpawnRateInSeconds;
    public float increaseSpawnRateInSeconds;
    float baseMaxSpawnRateInseconds;

	// Use this for initialization
	void Start () {
        baseMaxSpawnRateInseconds = maxSpawnRateInSeconds;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Function to spawn an enemy
    void SpawnEnemy()
    {
        //bottom-left point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        //top-right point of the screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //instantiate enemy
        GameObject anEnemy = (GameObject)Instantiate(EnemyGO);
        anEnemy.transform.position = new Vector2 (Random.Range(min.x + 0.255f, max.x - 0.255f), max.y); //set enemy position to the random x on top of screen

        //Schedule when to spawn next enemy
        ScheduleNextEnemySpawn();

    }

    void ScheduleNextEnemySpawn()
    {

        float spawnInNSeconds;

        if (maxSpawnRateInSeconds > minSpawnRateInSeconds)
        {
            //pick a number between 1 and maxSpawnRateInSeconds
            spawnInNSeconds = Random.Range(minSpawnRateInSeconds, maxSpawnRateInSeconds);

        }
        else
            spawnInNSeconds = 1f;
        Invoke("SpawnEnemy", spawnInNSeconds);
    }

    //Function to increase the difficulty of the game
    void IncreaseSpawnRate()
    {
        if (maxSpawnRateInSeconds > minSpawnRateInSeconds)
            maxSpawnRateInSeconds--;

        if (maxSpawnRateInSeconds == minSpawnRateInSeconds)
            CancelInvoke("IncreaseSpawnRate");

    }

    //Function to start enemy spawner
    public void ScheduleEnemySpawner()
    {
        maxSpawnRateInSeconds = baseMaxSpawnRateInseconds;
        Invoke("SpawnEnemy", maxSpawnRateInSeconds);
        //Increase spawn rate every 30 seconds
        InvokeRepeating("IncreaseSpawnRate", 0f, increaseSpawnRateInSeconds);
    }

    //Function to stop enemy spawner 
    public void UnscheduleEnemySpawner()
    {
        CancelInvoke("SpawnEnemy");
        CancelInvoke("IncreaseSpawnRate");
    }

}
