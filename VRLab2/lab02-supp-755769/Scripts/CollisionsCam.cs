using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionsCam : MonoBehaviour
{
    public GameObject manager;
    public bool insideSpawnRoom = false;
    void OnTriggerEnter(UnityEngine.Collider other)
    {
        //Chaseck for a match with the specific tag on any GameObject that collides with your GameObject
        if (other.gameObject.CompareTag("SpawnRoom"))
        {
            manager.GetComponent<ManagerScript>().EnterSpawnRoom();
        }
        else if (other.gameObject.CompareTag("DestroyRoom"))
        {
            manager.GetComponent<ManagerScript>().EnterDestroyRoom();
        }
        else if(other.gameObject.CompareTag("SkullRoom"))
        {
            manager.GetComponent<ManagerScript>().EnterSkullRoom();
        }
        else if (other.gameObject.CompareTag("Portal"))
        {
            manager.GetComponent<ManagerScript>().EnterPortal();
        }
    }

    void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("SpawnRoom"))
        {
            manager.GetComponent<ManagerScript>().ExitSpawnRoom();
        }
        else if (other.gameObject.CompareTag("DestroyRoom"))
        {
            manager.GetComponent<ManagerScript>().ExitDestroyRoom();
        }
        else if (other.gameObject.CompareTag("SkullRoom"))
        {
            manager.GetComponent<ManagerScript>().ExitSkullRoom();
        }
        else if (other.gameObject.CompareTag("Portal"))
        {
            manager.GetComponent<ManagerScript>().ExitPortal();
        }
    }
}
