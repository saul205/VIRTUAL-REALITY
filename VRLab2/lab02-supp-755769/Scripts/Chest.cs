using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    public List<Mesh> meshes = new List<Mesh>();
    private State state;
    private enum State
    {
        Closed,
        Open,
        Empty
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenChest()
    {
        GetComponent<MeshFilter>().mesh = Instantiate(meshes[1]);
    }
    public void TakeGold()
    {
        Transform coins = transform.Find("coins");
        Destroy(coins.gameObject);
    }

    public void Interact()
    {
        if (state == State.Closed)
        {
            OpenChest();
            state = State.Open;
        }
        else if (state == State.Open)
        {
            TakeGold();
            state = State.Empty;
        }
    }

    public string GetMessage()
    {
        if (state == State.Closed)
        {
            return "Press E to open";
        }
        else if (state == State.Open)
        {
            return "Press E to take gold";
        }
        else
        {
            return "";
        }
    }
}
