using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkullScript : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 10;
    public float minDistance = 1;
    public float speed = 5;

    public Vector3 center; // Center of the cube to spawn
    public Vector3 size; // Size of the cube to spawn

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerTr = new Vector3(player.position.x, 0, player.position.z);
        Vector3 transformTr = new Vector3(transform.position.x, 0, transform.position.z);
        while ((playerTr - transformTr).magnitude <= minDistance)
        {
            Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2));

            transform.position = pos;

            playerTr = new Vector3(player.position.x, 0, player.position.z);
            transformTr = new Vector3(transform.position.x, 0, transform.position.z);
        }

        float distanceRatio = 1 - Mathf.Clamp(((player.position - transform.position).magnitude - minDistance) / maxDistance, 0, 1);
        transform.Rotate(Vector3.up, speed * distanceRatio * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }
}
