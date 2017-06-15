using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO(robert): look into why character can sometimes jump an additional jump.
[RequireComponent(typeof(Rigidbody2D), typeof(EventController))]
public class PlayerJumpController : MonoBehaviour
{
    public float jumpForce = 150f;
    public int numberOfJumps = 1;
    public float maxJumpTime = 0.2f;

    private Rigidbody2D rigidbody2d;
    private PlayerControls playerControls;
    private GroundStatus groundStatus;
    private bool jump = false;
    private int jumpIndex;
    private float jumpTime = 0f;
    private bool highJumping = false;
    private PlayerJumpEvent jumpEvent;
    private float horizontalForce = 0f;
    private EventController eventController;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerControls = GetComponent<PlayerControls>();
        groundStatus = GetComponent<GroundStatus>();
        eventController = GetComponent<EventController>();
    }

    void Start()
    {
        jumpIndex = numberOfJumps;

        jumpEvent = new PlayerJumpEvent();
    }

    void OnEnable()
    {
        eventController.AddListener<WallJumpEvent>(OnWallJumpRequested);    
    }

    void OnDisable()
    {
        eventController.RemoveListener<WallJumpEvent>(OnWallJumpRequested);
    }

    void Update()
    {
        if(groundStatus.isGrounded)
        {
            jumpIndex = numberOfJumps;
            highJumping = false;
            jumpTime = 0f;
        }

        if(playerControls.IsJumpButtonPressed())
        {
            if (jumpIndex > 0)
            {
                jumpTime = 0f;
                jump = true;
            }

            highJumping = false;
        }
        else if (playerControls.IsJumpButtonHeld())
        {
            if (!groundStatus.isGrounded)
            {
                jumpTime += Time.deltaTime;

                if (jumpTime < maxJumpTime)
                {
                    jump = true;
                    highJumping = true;
                }
            }
        }
        else if(playerControls.IsJumpButtonReleased())
        {
            if(!groundStatus.isGrounded)
            {
                jumpTime = maxJumpTime;
            }
        }

    }

    void FixedUpdate()
    {
        if(jump)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0f);
            rigidbody2d.AddForce(new Vector2(horizontalForce, jumpForce));

            horizontalForce = 0f;

            if (!highJumping)
            { 
                eventController.Raise(jumpEvent);
                jumpIndex--;
            }

            jump = false;
        }
    }

    private void OnWallJumpRequested(WallJumpEvent e)
    {
        horizontalForce = e.status.directionToJump == FacingDirection.Right ? 
            e.status.horizontalForce : -e.status.horizontalForce;
        
        jump = true;
        jumpIndex = numberOfJumps;
        highJumping = false;
        jumpTime = 0f;
    }
}
