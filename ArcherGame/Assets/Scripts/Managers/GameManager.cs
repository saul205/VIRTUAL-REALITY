using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public List<Target> enemies = new List<Target>();
    public List<Target> targetsPrefabs;

    public Item keyPrefab;
    public List<GameObject> Keys;
    private int key_idx = 0;
    public Item correct_key;

    public int startingEnemies = 4;
    public float spawnRate;
    public float lastSpawnTime;

    public bool gameOver = false;

    public bool keyFound = false;
    public GameObject exit;
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

        key_idx = Random.Range(0, Keys.Count);
        correct_key = Keys[key_idx].GetComponent<Item>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver || player == null)
        {
            Application.Quit();
        }

        SpawnEnemies();

        //Remove dead enemies
        enemies.RemoveAll(x => x == null);
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

    public void PickKey(GameObject key)
    {
        Keys.Remove(key);
        keyFound = true;
    }

    public void TryKey(Item key)
    {
        if(key != null && key == correct_key)
        {
            gameOver = true;
        }
        keyFound = false;
    }
}
