using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float camSens = 1f;

    public Transform player;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float movX = Mouse.current.delta.x.ReadValue() * camSens * Time.deltaTime;
        float movY = Mouse.current.delta.y.ReadValue() * camSens * Time.deltaTime;

        var x = transform.localEulerAngles.x - movY;
        if (transform.localEulerAngles.x >= 270 && x < 270)
        {
            x = 270;
        }
        else if (transform.localEulerAngles.x <= 90 && x > 90)
            x = 90;

        transform.localEulerAngles = new Vector3(x, 0, 0);
        player.Rotate(Vector3.up * movX);
    }
}
