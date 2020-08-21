using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    GameManager gameManager;
    float waitTime;
    public float spawnTime;
    GameObject lastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Background").GetComponent<GameManager>();
        spawnTime = Random.Range(3f, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.levelStarted) waitTime += Time.deltaTime;
        if (waitTime >= spawnTime && !gameManager.levelEnd && lastSpawn == null && gameManager.playerAlive)
        {
            waitTime = 0;
            spawnTime = Random.Range(4f / Mathf.Clamp(1 + gameManager.level / 10, .5f, 4f), 8f / Mathf.Clamp(1 + gameManager.level / 10, .9f, 8f));
            Spawn();
        }
    }

    void Spawn()
    {
        float largeChance = 1 + (gameManager.level / 2);
        float mediumChance = 1 + gameManager.level * 1.5f;
        float randomLocation = Random.Range(transform.position.x - 2, transform.position.x + 2);

        float spawnRandom = Random.Range(0f, 100f);
        if (spawnRandom >= 100f - largeChance)
        {
            GameObject newSpawn = Instantiate(gameManager.enemyLarge[Random.Range(0, gameManager.enemyLarge.Length)], transform.position, transform.rotation);
            lastSpawn = newSpawn;
        }
        else if (spawnRandom >= 100f - mediumChance)
        {
            Instantiate(gameManager.enemyMedium[Random.Range(0, gameManager.enemyMedium.Length)], transform.position, transform.rotation);
        }
        else
        {
            Instantiate(gameManager.enemySmall[Random.Range(0, gameManager.enemySmall.Length)], transform.position, transform.rotation);
        }
    }
}
