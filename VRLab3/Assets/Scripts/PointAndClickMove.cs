using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClickMove : MonoBehaviour
{
    public LayerMask floorMask;
    public GameObject markerPrefab;
    public GameObject player;
    public float playerHeight = 0.7f;

    private GameObject currentMarker;
    private bool mark = false;
    private bool move = false;
    public bool isMoving = false;

    public Vector3 currentSpeed = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mark || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f, floorMask))
            {
                if (currentMarker != null)
                {
                    Destroy(currentMarker);
                }
                currentMarker = Instantiate(markerPrefab, hit.point, Quaternion.identity);
                currentMarker.transform.up = hit.normal;
            }

            mark = false;
        }

        if(move || Input.GetKeyDown(KeyCode.Joystick1Button1) && currentMarker != null)
        {
            isMoving = true;
            move = false;
        }
        else if(isMoving && currentMarker != null)
        {
            var direction = currentMarker.transform.position - player.transform.position;
            direction.y = 0;
            var normDirection = direction.normalized;

            var speed = player.GetComponent<PlayerController>().movSpeed / 2;
            if(speed * Time.deltaTime < direction.magnitude)
            { 
                speed = direction.magnitude / Time.deltaTime;
            }

            currentSpeed = normDirection * speed;
            if(direction.magnitude < 0.05)
            {
                isMoving = false;
                currentSpeed = Vector3.zero;
                Destroy(currentMarker);
            }
        }
    }

    public void OnTeleportMark()
    {
        mark = true;
    }

    public void OnTeleport()
    {
        move = true;
    }
}
