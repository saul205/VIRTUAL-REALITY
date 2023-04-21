using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movSpeed = 3f;
    public bool canMove = true;

    private CamMovement cm;
    // Start is called before the first frame update
    void Awake()
    {
        cm = new CamMovement();
    }

    private void OnEnable()
    {
        cm.CamMov.Enable();
    }

    private void OnDisable()
    {
        cm.CamMov.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Vector2 mov = cm.CamMov.Move.ReadValue<Vector2>();

            var step = movSpeed * Time.deltaTime; // calculate distance to move
            Vector3 tr = transform.position;
            GetComponent<CharacterController>().Move(transform.localToWorldMatrix * new Vector3(mov.x, 0, mov.y) * step);
        }
    }
}
