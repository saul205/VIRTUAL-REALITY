using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
/*
 * D = Button1
 * C = Button2
 * A/Trigger1 = Button3
 * Trigger2 = Button0
 */

public class PlayerController : MonoBehaviour
{
    public float movSpeed = 5f;
    public float Gravity = 9.82f;
    public bool IsGrounded = true;
    public Transform groundCheck;
    public Transform ballPosition;
    public LayerMask GroundLayers;

    public Text text;

    private Vector3 Velocity = Vector3.zero;
    private bool jump = false;

    public PlayerInput pm;

    private float LastJump = 0f;
    private float JumpingLandPrevention = .2f;

    private BasketBall ball;
    private bool isHolding = false;
    public PointAndClickMove pcm;

    // Start is called before the first frame update
    void Awake()
    {
        pm = new PlayerInput();
        gameObject.GetComponent<CharacterController>().detectCollisions = false;

        var myAction = new InputAction(binding: "/*/<button>");
        myAction.performed += ctx => ActionPerf(ctx);
        myAction.Enable();
    }

    public void ActionPerf(InputAction.CallbackContext ctx)
    {
        text.text = ctx.control.path;
    }

    private void OnEnable()
    {
        pm.Player.Enable();
    }

    private void OnDisable()
    {
        pm.Player.Disable();
    }

    public void OnJump()
    {
        jump = !jump;
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
        }
    }
    public void CorrectBallPosition()
    {
        if(isHolding)
        {
            var ratio = Vector3.Lerp(ball.gameObject.transform.position, ballPosition.position, 1f);
            ball.gameObject.transform.position = ratio;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();

        Vector2 mov = pm.Player.Move.ReadValue<Vector2>();
        Vector3 tr = transform.position;

        if (pcm.isMoving)
        {
            Velocity = pcm.currentSpeed + new Vector3(0, Velocity.y, 0);
        }
        else
        {
            var y = Velocity.y;
            Velocity = Camera.main.transform.localToWorldMatrix * new Vector3(mov.x, 0, mov.y);
            Velocity.y = 0;
            Velocity = Velocity.normalized;
            Velocity *= movSpeed;
            Velocity.y = y;
        }

        if (IsGrounded)
        {
            Velocity.y = 0;
            if (jump || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                Velocity += Vector3.up * 4;
                IsGrounded = false;
                LastJump = Time.time;
            }
        }
        else
            Velocity += Vector3.down * Gravity * Time.deltaTime;

        
        gameObject.GetComponent<CharacterController>().Move(Velocity * Time.deltaTime);

        if(isHolding && (ball.gameObject.transform.position - ballPosition.position).magnitude > 1)
        {
            DropBall();
        }

        if (shoot || Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            HandleShoot();
        }

        CorrectBallPosition();
        ResetInput();
    }

    public float throwForce = 10;

    public void DropBall()
    {
        var rb = ball.gameObject.GetComponent<Rigidbody>();

        rb.useGravity = true;
        isHolding = false;
        ball.gameObject.transform.SetParent(null);
        ball = null;
    }
    public void HandleShoot()
    {
        if (isHolding)
        {
            var cam = Camera.main;
            var rb = ball.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);

            ball.Shoot();
            DropBall();
        }
        shoot = false;
    }

    public void ResetInput()
    {

    }

    private void PickBall(BasketBall _ball)
    {
        if (isHolding)
        {
            return;
        }
        else
        {
            ball = _ball;
            ball.gameObject.transform.SetParent(ballPosition, false);
            ball.gameObject.GetComponent<Rigidbody>().useGravity = false;
            ball.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.gameObject.transform.localPosition = Vector3.zero;
            isHolding = true;
        }
    }

    public void OnCollisionEnter(Collision hit)
    {
        // Recoger pelota
        if (hit.gameObject.CompareTag("Spheres"))
        {
            BasketBall ball = hit.gameObject.GetComponent<BasketBall>();
            if (ball != null)
            {
                PickBall(ball);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Spheres"))
        {
            BasketBall ball = other.gameObject.GetComponent<BasketBall>();
            if (ball != null)
            {
                PickBall(ball);
            }
        }
    }

    private bool shoot = false;
    public void OnShoot()
    {
        shoot = true;
    }

}
