using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    /// <summary>
    /// This script handles the spawning of enemies,
    /// enemies spawn within a boundary at random X and Y locations 
    /// </summary>
    //enemy prefab ready to spawn in game
    public GameObject enemyPrefab;

    //closest enemies can spawn
    public int minSpawnDistance;

    //furthest enemies can spawn
    public int maxSpawnDistance;

    public int spawnTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        //start spawning enemies
        StartCoroutine(enemySpawner());
    }


    private void spawnEnemy()
    {
        //get a location at a random x within -100 and 100, random y, between -20 and 20 and along the z between the max and min distances 
        var position = new Vector3(Random.Range(-100f, 100f), Random.Range(-20f, 20f), (GameObject.FindGameObjectWithTag("Player").transform.position.z + Random.Range(minSpawnDistance, maxSpawnDistance)));

        //spawn the enemy at this position
        Instantiate(enemyPrefab,position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), 0),null);
    }

    //call the spawnEnemy method then wait for a random time between 0 and spawnTime
    IEnumerator enemySpawner()
    {
        //run constantly
        while (true)
        {
            //run the spawn enemy method
            spawnEnemy();

            //pause for a random time between 0 and spawntime
            yield return new WaitForSeconds(Random.Range(0, spawnTime));
        }

    }
}
