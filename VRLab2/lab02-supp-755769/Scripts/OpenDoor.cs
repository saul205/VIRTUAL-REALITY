using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    public float speed = 1f;
    public bool openDoor = false;
    void OnTriggerEnter(UnityEngine.Collider other)
    {
        //Chaseck for a match with the specific tag on any GameObject that collides with your GameObject
        if (other.gameObject.CompareTag("Player"))
        {
            openDoor = true;
        }
    }

    void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            openDoor = false;
        }
    }

    private void Update()
    {
        Transform leftDoor = transform.Find("Left");
        Transform rightDoor = transform.Find("Right");

        if (openDoor)
        {
            leftDoor.localPosition = Vector3.MoveTowards(leftDoor.localPosition, new Vector3(2.25f, 0, 0), speed * Time.deltaTime);
            rightDoor.localPosition = Vector3.MoveTowards(rightDoor.localPosition, new Vector3(-2.25f, 0, 0), speed * Time.deltaTime);
        }
        else
        {
            leftDoor.localPosition = Vector3.MoveTowards(leftDoor.localPosition, new Vector3(.75f, 0, 0), speed * Time.deltaTime);
            rightDoor.localPosition = Vector3.MoveTowards(rightDoor.localPosition, new Vector3(-.75f, 0, 0), speed * Time.deltaTime);
        }
    }
}
