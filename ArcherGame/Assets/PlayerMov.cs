using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    public float movSpeed = 5f;
    public bool canMove = true;

    private PlayerControl pm;
    // Start is called before the first frame update
    void Awake()
    {
        pm = new PlayerControl();
    }

    private void OnEnable()
    {
        pm.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        pm.PlayerMovement.Disable();
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 mov = pm.PlayerMovement.Movement.ReadValue<Vector2>();

        var step = movSpeed * Time.deltaTime; // calculate distance to move
        Vector3 tr = transform.position;
        GetComponent<CharacterController>().Move(transform.localToWorldMatrix * new Vector3(mov.x, 0, mov.y) * step);

        /*transform.Translate(new Vector3(mov.x, 0, mov.y) * step);
        tr.x = transform.position.x;
        tr.z = transform.position.z;
        transform.position = tr;*/
    }
}
