using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemSpawner : MonoBehaviour
{
    // Properties that can be changed from the Inspetor tab
    public GameObject ItemToSpawn; // Item to be spawned
    public Vector3 center; // Center of the cube to spawn
    public Vector3 size; // Size of the cube to spawn
                         // Start is called before the first frame update

    public int spawnFrames = 60;
    private int frameCounter = 0;
    public float spawnTime = 1.0f;
    private float time = 0.0f;

    public bool spawnPending = false;
    void Start()
    {
        SpawnItem();
        frameCounter = spawnFrames;
    }

    public void OnSpawn()
    {
        Debug.Log("pressed");
        if(GetComponent<ManagerScript>().SpawnAllowed)
            spawnPending = !spawnPending;

        time = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnPending && GetComponent<ManagerScript>().SpawnAllowed)
        {  // When Q is pressed, an item is spawned
            SpawnItem();
        }
    }
    public void SpawnItem()
    {
        /*if(frameCounter > 0)
        {
            frameCounter--;
            return;
        }*/

        if(time - Time.deltaTime > 0)
        {
            time = time - Time.deltaTime;
            return;
        }

        // Position to spawn
        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2),
        Random.Range(-size.y / 2, size.y / 2),
        Random.Range(-size.z / 2, size.z / 2));
        // Instantiate the object
        Instantiate(ItemToSpawn, pos, Quaternion.identity);

        //frameCounter = spawnFrames;
        time = spawnTime;
    }

    // Debug funcion. In the Scene tab you can see a Red Box, which is the
    // volume where the object is going to be spawned.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }
}
