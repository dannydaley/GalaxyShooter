using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{

    public GameObject enemyPrefab;

    public int minSpawnDistance;

    public int maxSpawnDistance;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(enemySpawner());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnEnemy()
    {
        var position = new Vector3(Random.Range(-100f, 100f), Random.Range(-20f, 20f), (GameObject.FindGameObjectWithTag("Player").transform.position.z + Random.Range(minSpawnDistance, maxSpawnDistance)));
        Instantiate(enemyPrefab,position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), 0),null);
    }

    IEnumerator enemySpawner()
    {
        while (true)
        {
        spawnEnemy();
        yield return new WaitForSeconds(Random.Range(0, 1));
        }

    }
}
