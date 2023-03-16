using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    public TextMeshProUGUI m_TextMeshProUGUI;
    public TextMeshProUGUI alerts;
    public Camera cam;
    public PlayerMovement player;
    public float rotateSpeed = 10f;
    public float startTransportation;

    private bool interact = false;
    private State state;

    private enum State
    {
        None,
        InSpawnRoom,
        InDestroyRoom,
        InSkullRoom, 
        Portal, 
        Transporting
    }

    public void OnOpen()
    {
        Debug.Log("pressed");
        interact = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.InSpawnRoom)
        {
            if (GetComponent<ItemSpawner>().spawnPending)
                alerts.text = "Press Q to stop spawning swords";
            else
                alerts.text = "Press Q to spawn swords";
        }
        else if(state == State.InDestroyRoom)
        {
            alerts.text = "Look at an object to destroy it";
        }
        else if (state == State.InSkullRoom)
        {
            alerts.text = "Try to take the skull";
        }
        else if (state == State.Portal)
        {
            alerts.text = "Press E to transport to a different dimension";
            if (hasInteracted())
            {
                state = State.Transporting;
                player.canMove = false;
                startTransportation = Time.time;
            }
        }
        else if (state == State.Transporting)
        {
            player.transform.Rotate(Vector3.up, rotateSpeed * (Time.time - startTransportation));
            if(Time.time - startTransportation > 3)
            {
                SceneManager.LoadScene("ShootingRange");
            }
        }
        else
        {
            alerts.text = "";
        }

        if (cam.GetComponent<CheckObject>().interactable != null)
        {
            m_TextMeshProUGUI.text = cam.GetComponent<CheckObject>().interactable.GetComponent<IInteractable>().GetMessage();
            if (hasInteracted())
            {
                cam.GetComponent<CheckObject>().interactable.GetComponent<IInteractable>().Interact();
            }
        }
        else
        {
            m_TextMeshProUGUI.text = "";
        }

        interact = false;
    }

    private bool hasInteracted()
    {
        switch (state)
        {
            case State.InDestroyRoom: 
                return true; 
            default:
                return interact;
        }
    }

    public void EnterSpawnRoom()
    {
        state = State.InSpawnRoom;
    }
    public void ExitSpawnRoom()
    {
        state = State.None;
    }

    public void EnterDestroyRoom()
    {
        state = State.InDestroyRoom;
        cam.GetComponent<CheckObject>().LookFor(LayerMask.GetMask("Destructible"), 20);
    }
    public void ExitDestroyRoom()
    {
        state = State.None;
        cam.GetComponent<CheckObject>().LookFor(LayerMask.GetMask("Chest"), 1);
    }

    public void EnterSkullRoom()
    {
        state = State.InSkullRoom;
    }
    public void ExitSkullRoom()
    {
        state = State.None;
    }

    public void EnterPortal()
    {
        state = State.Portal;
    }
    public void ExitPortal()
    {
        state = State.None;
    }

    public bool SpawnAllowed => state == State.InSpawnRoom;
    public bool DestroyAllowed => state == State.InDestroyRoom;
}
