using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WallClingStatus
{
    bool isClingingToWall { get; }
}

[RequireComponent(typeof(PlayerControls),
    typeof(PlayerDirection))]
[RequireComponent(typeof(PlayerMotor), typeof(EventController))]
public class PlayerWallCling : MonoBehaviour, WallClingStatus
{
    [Range(0f, 1f)]
    public float velocityDampener = 0.05f;
    public float downwardsForceOnclingRelease = 20f;

    public bool isClingingToWall { get; private set; }

    private PlayerControls playerControls;
    private PlayerDirection facing;
    private PlayerMotor motor;
    private PlayerWallClingEvent clingEvent;
    private EventController events;
    private bool grounded = false;
    private bool touchingWall = false;

    void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        facing = GetComponent<PlayerDirection>();
        motor = GetComponent<PlayerMotor>();
        events = GetComponent<EventController>();
    }

    void Start()
    {
        clingEvent = new PlayerWallClingEvent(this);
    }

    void OnEnable()
    {
        events.AddListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
        events.AddListener<PlayerWallStatusEventChangeEvent>(OnWallStatusChanged);
    }

    void OnDisable()
    {
        events.RemoveListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
        events.RemoveListener<PlayerWallStatusEventChangeEvent>(OnWallStatusChanged);
    }

    void Update()
    {
        if (grounded || !touchingWall)
        {
            return;
        }

        if (playerControls.IsNoMovementControlPressed())
        {
            // If the player was clinging to a wall but has released all movement controls.
            if (isClingingToWall)
            {
                isClingingToWall = false;

                events.Raise(clingEvent);

                motor.AddForce(Vector2.down * downwardsForceOnclingRelease);
            }
        }
        else
        {
            var move = playerControls.GetMovement();

            if ((facing.currentDirection == FacingDirection.Left && move < 0f) ||
                (facing.currentDirection == FacingDirection.Right && move > 0f))
            {
                bool prevStatus = isClingingToWall;

                isClingingToWall = true;
                motor.velocity = new Vector2(motor.velocity.x, motor.velocity.y * velocityDampener);

                if (prevStatus != isClingingToWall)
                {
                    events.Raise(clingEvent);
                }
            }
        }

    }

    private void OnGroundStatusChanged(PlayerGroundStatusChangeEvent e)
    {
        grounded = e.groundStatus == GroundStatus.Grounded;

        if (grounded && isClingingToWall)
        {
            isClingingToWall = false;

            events.Raise(clingEvent);
        }
    }

    private void OnWallStatusChanged(PlayerWallStatusEventChangeEvent e)
    {
        touchingWall = e.isTouchingWall;

        if (!touchingWall && isClingingToWall)
        {
            isClingingToWall = false;

            events.Raise(clingEvent);
        }

    }
}
