using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public List<Target> enemies = new List<Target>();
    public List<Target> targetsPrefabs;

    public int startingEnemies = 4;
    public float spawnRate;
    public float lastSpawnTime;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;  i < startingEnemies; i++)
        {
            var randIndex = UnityEngine.Random.Range(0, targetsPrefabs.Count);
            var randX = UnityEngine.Random.Range(-10, 10);
            var randY = UnityEngine.Random.Range(-10, 10);
            var enemy = Instantiate(targetsPrefabs[randIndex], new Vector3(randX, 0, randY), Quaternion.identity);
            enemy.target = player.gameObject;
            enemies.Add(enemy);
        }
        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (Time.time - lastSpawnTime > 1 / spawnRate)
        {
            var randIndex = UnityEngine.Random.Range(0, targetsPrefabs.Count);
            var randX = UnityEngine.Random.Range(-10, 10);
            var randY = UnityEngine.Random.Range(-10, 10);
            var enemy = Instantiate(targetsPrefabs[randIndex], new Vector3(randX, 0, randY), Quaternion.identity);
            enemy.target = player.gameObject;
            enemies.Add(enemy);
            lastSpawnTime = Time.time;
        }
    }

    public Tuple<int, int> GetPlayerHPData()
    {
        return new Tuple<int, int> (player.Hp, player.MaxHp);
    }
}
