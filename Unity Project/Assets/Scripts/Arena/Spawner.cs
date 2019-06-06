using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Spawner</c> is used only once per arena to automate enemies spawning
/// </summary>
public class Spawner : MonoBehaviour
{

    public GameObject[] enemies;
    public float startSpeed;
    public float endSpeed;
    public float decreasePerMinute;

    private Transform[] spawners;
    private float currentSpawnRate;
    private float timeSinceLastSpawn = 0;

    private void Awake()
    {
        spawners = GetComponentsInChildren<Transform>();
        currentSpawnRate = startSpeed;
    }

    private void Update()
    {
        // continue decreasing time interval between enemies spawning until final interval is reached
        if (currentSpawnRate > endSpeed)
        {
            currentSpawnRate = currentSpawnRate - decreasePerMinute * (Time.deltaTime / 60f);
            if (currentSpawnRate < endSpeed)
            {
                currentSpawnRate = endSpeed;
            }
        }

        timeSinceLastSpawn += Time.deltaTime;

        // spawn enemies if necessary time interval has passed
        if (timeSinceLastSpawn >= currentSpawnRate)
        {
            timeSinceLastSpawn = 0;
            Instantiate(enemies[Random.Range(0, enemies.Length)], (Vector2)spawners[Random.Range(0, spawners.Length)].position, new Quaternion());
        }
    }
}
