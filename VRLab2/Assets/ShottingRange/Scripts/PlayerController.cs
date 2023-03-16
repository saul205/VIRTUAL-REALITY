using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: Damageable
{
    public float movSpeed = 5f;
    public float Gravity = 9.82f;
    private bool IsGrounded = true;
    public Transform groundCheck;
    public LayerMask GroundLayers;


    private Vector3 Velocity = Vector3.zero;
    private bool jump = false;

    private PlayerControl pm;

    private float LastJump = 0f;
    private float JumpingLandPrevention = .2f;
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

    public void OnJump()
    {
        jump = !jump;
    }

    public void CheckGround()
    {
        IsGrounded = false;

        if(Time.time >= LastJump + JumpingLandPrevention)
        {
            var Colliders = Physics.OverlapSphere(groundCheck.position, .1f, GroundLayers);
            foreach(var collider in Colliders)
            {
                if (Vector3.Angle(collider.transform.up, Vector3.up) < 10)
                {
                    IsGrounded = true;
                }
            }
        }
    }

    public void CheckAlive()
    {
        if(Hp <= 0)
        {
            Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckAlive();
        CheckGround();

        Vector2 mov = pm.PlayerMovement.Movement.ReadValue<Vector2>();
        Vector3 tr = transform.position;

        Velocity = transform.localToWorldMatrix * new Vector3(mov.x, 0, mov.y) * movSpeed + new Vector4(0, Velocity.y, 0);

        if (IsGrounded)
        {
            Velocity.y = 0;
            if (jump)
            {
                Velocity += Vector3.up * 4;
                IsGrounded = false;
                LastJump = Time.time;
            }
        }
        else
            Velocity += Vector3.down * Gravity * Time.deltaTime;

        gameObject.GetComponent<CharacterController>().Move(Velocity * Time.deltaTime);

        ResetInput();
    }

    public void ResetInput()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, .2f);
    }
}
