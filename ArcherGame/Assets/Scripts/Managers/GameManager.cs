using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public List<Target> enemies = new List<Target>();
    public List<Target> targetsPrefabs;

    public float spawnArea = 50f;
    public List<GameObject> spawns;

    public Item keyPrefab;
    public List<GameObject> Keys;
    private int key_idx = 0;
    public Item correct_key;

    public int startingEnemies = 4;
    public float spawnRate;
    public int nSpawns = 2;
    private float lastSpawnTime = -Mathf.Infinity;
    public int spawnLimit = 50;

    public bool gameOver = false;
    public AudioClip breakClip;
    public AudioClip openClip;
    private float openTime = 0;

    public bool keyFound = false;
    public GameObject exit;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> availableSpawns = spawns.Where(x => (x.transform.position - player.transform.position).magnitude <= spawnArea).ToList();
        if (availableSpawns.Any())
            SpawnEnemies(availableSpawns, startingEnemies);

        lastSpawnTime = Time.time;

        key_idx = Random.Range(0, Keys.Count);
        if(Keys.Count > 0)
            correct_key = Keys[key_idx].GetComponent<Item>();
    }


    public bool Tutorial = false;
    // Update is called once per frame
    void Update()
    {
        if (gameOver && Time.time >= openTime + openClip.length)
        {
            if (Tutorial)
                SceneManager.LoadScene("Prueba");
            else
                SceneManager.LoadScene("End");
        }

        if(player == null)
        {
            Application.Quit();
        }
        List<Vector3> pos = spawns.Select(x => x.transform.position).ToList();
        var a = pos.Select(x => (player.transform.position - x).magnitude).ToList();
        List<GameObject> availableSpawns = spawns.Where(x => (x.transform.position - player.transform.position).magnitude <= spawnArea).ToList();
        if(availableSpawns.Any())
            SpawnEnemies(availableSpawns);

        //Remove dead enemies
        enemies.RemoveAll(x => x == null);
    }

    public void SpawnEnemies(List<GameObject> availableSpawns, int nEnemies = -1)
    {
        if (nEnemies < 0)
            nEnemies = nSpawns;

        List<int> spawned = new List<int>();
        if (Time.time - lastSpawnTime >= 1 / spawnRate && enemies.Count < spawnLimit)
        {
            for(int i = 0; i < nEnemies && i < availableSpawns.Count; i++)
            {
                int index = -1;
                do
                {
                    index = Random.Range(0, availableSpawns.Count);
                } while (spawned.Contains(index));

                spawned.Add(index);

                int randIndex = Random.Range(0, targetsPrefabs.Count);

                var enemy = Instantiate(targetsPrefabs[randIndex], availableSpawns[index].transform.position, Quaternion.identity);
                enemy.target = player.gameObject;
                enemies.Add(enemy);
            }
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
            AudioSource.PlayClipAtPoint(openClip, player.transform.position);
            gameOver = true;
            openTime = Time.time;
        }
        else if(key != null)
        {
            AudioSource.PlayClipAtPoint(breakClip, player.transform.position);
        }
        keyFound = false;
    }
}
