using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Damageable
{
    public float movSpeed = 5f;
    public float Gravity = 9.82f;
    private bool IsGrounded = true;
    public Transform groundCheck;
    public LayerMask GroundLayers;

    public float iTime = 1;
    public float current_iTime = 0;
    public override bool CanTakeDmg => current_iTime == 0;

    private Vector3 Velocity = Vector3.zero;

    private PlayerControl pm;

    private float LastJump = 0f;
    private float JumpingLandPrevention = .2f;

    public InputManager InputManager;
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

    public void CheckGround()
    {
        IsGrounded = false;

        if (Time.time >= LastJump + JumpingLandPrevention)
        {
            RaycastHit hit;
            if (Physics.SphereCast(groundCheck.position, .5f, Vector3.down, out hit, .1f, GroundLayers))
            {
                if (Vector3.Angle(hit.normal, Vector3.up) < 15)
                {
                    IsGrounded = true;
                }
            }
            /*
            var Colliders = Physics.OverlapSphere(groundCheck.position, .1f, GroundLayers);
            foreach(var collider in Colliders)
            {
                if (Vector3.Angle(collider.transform.up, Vector3.up) < 10)
                {
                    IsGrounded = true;
                }
            }*/
        }
    }

    public void CheckAlive()
    {
        if (Hp <= 0)
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

        var y = Velocity.y;
        Velocity = Camera.main.transform.localToWorldMatrix * new Vector3(mov.x, 0, mov.y);
        Velocity.y = 0;
        Velocity = Velocity.normalized;
        Velocity *= movSpeed;
        Velocity.y = y;

        if (IsGrounded)
        {
            Velocity.y = 0;
            if (InputManager.GetJumpInput())
            {
                Velocity += Vector3.up * 4;
                IsGrounded = false;
                LastJump = Time.time;
            }
        }
        else
            Velocity += Vector3.down * Gravity * Time.deltaTime;

        gameObject.GetComponent<CharacterController>().Move(Velocity * Time.deltaTime);

        current_iTime = Mathf.Max(current_iTime - Time.deltaTime, 0);

        ResetInput();
    }

    public void ResetInput()
    {

    }

    public override void AfterHit()
    {
        base.AfterHit();
        current_iTime = iTime;
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.rigidbody != null)
            //hit.rigidbody.AddForce(hit.moveDirection.normalized * 5, ForceMode.Force);
            hit.rigidbody.AddForceAtPosition(hit.moveDirection.normalized * 5, hit.point, ForceMode.Force);
    }
}
