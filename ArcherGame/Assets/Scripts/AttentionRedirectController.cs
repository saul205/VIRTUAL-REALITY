using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionRedirectController : MonoBehaviour
{
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Find closest key
        var pos = transform.position;
        foreach(var key in gameManager.keys)
        {

        }
        // Create the indicator
    }
}
