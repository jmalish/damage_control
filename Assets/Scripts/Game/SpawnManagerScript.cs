using UnityEngine;
using System.Collections.Generic;

public class SpawnManagerScript : MonoBehaviour {

    float playerHealth;
    GameObject player;
    public GameObject enemySmall, enemyMedium, enemyLarge, asteroidSmall, asteroidLarge;
    public List<GameObject> enemies, asteroids;
    float spawnTimer;
    public static bool canSpawn = false;

    void Start()
    {
        canSpawn = false

        player = GameObject.FindGameObjectWithTag("Player");

        enemies.Add(enemySmall);
        enemies.Add(enemyMedium);
        enemies.Add(enemyLarge);

        asteroids.Add(asteroidSmall);
        asteroids.Add(asteroidLarge);

        InvokeRepeating("SpawnEnemy", 1, 1);
        InvokeRepeating("SpawnAsteroid", 1, 10);
    }

    void SpawnEnemy()
    {
        playerHealth = PlayerScript.health;
        int score = ScoreManager.score;

        if (Time.time > spawnTimer)
        {
            if (playerHealth < 1) // if player is dead, no reason to spawn new enemies
            {
                return;
            }

            if (canSpawn)
            {
                int enemyID = Random.Range(0, enemies.Count); // choose which enemy to spawn

                // get player pos
                float playerX = player.transform.position.x;
                float playerY = player.transform.position.y;

                // generate random spawn
                float spawnX = Random.Range(playerX - 30, playerX + 30);
                float spawnY = Random.Range(playerY - 30, playerY + 30);

                Vector3 spawnPos = new Vector3(spawnX, spawnY);

                Instantiate(enemies[enemyID], spawnPos, Quaternion.identity);

                if (score < 500)
                {
                    spawnTimer = Time.time + 10;
                }
                else if (score >= 500 && score < 1000)
                {
                    spawnTimer = Time.time + 5;
                }
                else if (score >= 2000)
                {
                    spawnTimer = Time.time + 3;
                }
                else if (score >= 3000)
                {
                    spawnTimer = Time.time + 2;
                }
            }
        }
    }

    void SpawnAsteroid()
    {
        playerHealth = PlayerScript.health;

        if (playerHealth < 1) // if player is dead, no reason to spawn
        {
            return;
        }

        if (!PlayerScript.tutorial)
        {
            int asteroidID = Random.Range(0, asteroids.Count); // choose which asteroid to spawn

            // get player pos
            float playerX = player.transform.position.x;
            float playerY = player.transform.position.y;

            // generate random spawn
            float spawnX = Random.Range(playerX - 30, playerX + 30);
            float spawnY = Random.Range(playerY - 30, playerY + 30);

            Vector3 spawnPos = new Vector3(spawnX, spawnY);

            Instantiate(asteroids[asteroidID], spawnPos, Quaternion.identity);
        }
    }
}
