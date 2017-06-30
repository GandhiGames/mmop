using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor), typeof(EventController))]
public class PlayerJumpController : MonoBehaviour
{
    public float jumpForce = 150f;
    public int numberOfJumps = 1;
    public float maxJumpTime = 0.2f;

    private PlayerMotor motor;
    private PlayerControls playerControls;
    private EventController events;

    private bool canJump = true;
    private bool jump = false;
    private int jumpIndex;
    private float jumpTime = 0f;
    private bool jumpExtension = false;
    private PlayerJumpEvent jumpEvent;
    private float horizontalForce = 0f;
    private bool queuedJump = false;
    private GroundStatus groundStatus;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        playerControls = GetComponent<PlayerControls>();
        events = GetComponent<EventController>();
    }

    void Start()
    {
        jumpIndex = numberOfJumps;

        jumpEvent = new PlayerJumpEvent();

        groundStatus = GroundStatus.None;
    }

    void OnEnable()
    {
        events.AddListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
        events.AddListener<WallJumpEvent>(OnWallJumpRequested);
        events.AddListener<PlayerCrouchEvent>(OnCrouchStatusChanged);
    }

    void OnDisable()
    {
        events.RemoveListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
        events.RemoveListener<WallJumpEvent>(OnWallJumpRequested);
        events.RemoveListener<PlayerCrouchEvent>(OnCrouchStatusChanged);
    }

    //TODO: clamp max y velocity.
    void Update()
    {
        if(!canJump)
        {
            return;
        }

        // If we have queued a jump when we were almost grounded,
        // and we are now grounded and still requesting to jump.
        if (queuedJump && groundStatus == GroundStatus.Grounded &&
            playerControls.IsJumpButtonHeld())
        {
            print("Queued jump");
            jumpTime = 0f;
            jump = true;
            jumpExtension = false;
        }

        // We are still waiting on the queued jump so no need to go any further.
        if (queuedJump && playerControls.IsJumpButtonHeld())
        {
            return;
        }

        // This gives some wiggle room for when a player can first attempt a jump.
        // It allows the player to be able to press the jump button slightly before the
        // characters has hit the ground and to queue a jump for when contact with the
        // ground is made. It makes the jump feel more reactive.
        if (groundStatus == GroundStatus.AlmostGrounded &&
            playerControls.IsJumpButtonPressed())
        {
            queuedJump = true;

            return;
        }


        if (playerControls.IsJumpButtonPressed())
        {
            // It does not matter whether we are grounded or not,
            // as long as the jump index is greater than 0 we want to jump.
            if (jumpIndex > 0)
            {
                jumpTime = 0f;
                jump = true;
            }

            // To signify we've started a new jump we need to reset this.
            jumpExtension = false;
        }
        else if (playerControls.IsJumpButtonHeld())
        {
            // Allows for a variable height jump by continually adding force
            // while the player holds down the jump button.
            if (groundStatus != GroundStatus.Grounded)
            {
                jumpTime += Time.deltaTime;

                if (jumpTime <= maxJumpTime)
                {
                    jump = true;
                    jumpExtension = true;
                }
            }
        }
        else if (playerControls.IsJumpButtonReleased())
        {
            if (groundStatus != GroundStatus.Grounded)
            {

                // Disables any further input to be registered as an extended
                // jump for the current jump index. Any further jump attempts will 
                // only be successful if the player has another jump or touches the ground.
                jumpTime = maxJumpTime;
            }
        }

    }

    void FixedUpdate()
    {
        if(jump)
        {
            // Reset current velocity to start a new jump.
            motor.velocity = new Vector2(motor.velocity.x, 0f);

            // Horizontal force will be added if the player is wall jumping.
            motor.AddForce(new Vector2(horizontalForce, jumpForce));

            // Horizontal force is reset after each jump so if the player attempts
            // any further non wall jumps, they will not be moved sidways.
            horizontalForce = 0f;

            // We only raise a jump event and decrement the jump index at the start of a new jump.
            // So we need to check we are not extending the current jump.
            if (!jumpExtension)
            { 
                events.Raise(jumpEvent);
                jumpIndex--;
            }

            jump = false;
            queuedJump = false;
        }
    }

    private void OnGroundStatusChanged(PlayerGroundStatusChangeEvent e)
    {
        groundStatus = e.groundStatus;

        if (groundStatus == GroundStatus.Grounded)
        {
            // If we are grounded we want to reset jump variables to default.
            ResetJump();
        }
    }

    private void OnWallJumpRequested(WallJumpEvent e)
    {
        // Add a horizontal force dependent on facing direction.
        horizontalForce = e.status.directionToJump == FacingDirection.Right ? 
            e.status.horizontalForce : -e.status.horizontalForce;
        
        jump = true;

        ResetJump();
    }

    /// <summary>
    /// Prevents the player from jumping if they are crouched.
    /// </summary>
    /// <param name="e"></param>
    private void OnCrouchStatusChanged(PlayerCrouchEvent e)
    {
        canJump = !e.status.isCrouching;
    }

    /// <summary>
    /// Resets jump attributes to their default values.
    /// This will ensure that the next jump performed is registered
    /// as the first jump.
    /// </summary>
    private void ResetJump()
    {
        jumpIndex = numberOfJumps;
        jumpExtension = false;
        jumpTime = 0f;
    }
}
