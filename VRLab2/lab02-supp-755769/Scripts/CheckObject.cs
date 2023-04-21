using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CheckObject : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject interactable;

    public float range = 1.0f;
    public int layerMask;
    void Start()
    {
        layerMask = LayerMask.GetMask("Chest");
    }

    public void LookFor(int _layerMask, float _range)
    {
        layerMask = _layerMask;
        range = _range; 
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(transform.position, transform.forward, out hit, range, layerMask))
        {
            interactable = hit.collider.gameObject;
        }
        else
        {
            interactable = null;
        }
        
    }
}
