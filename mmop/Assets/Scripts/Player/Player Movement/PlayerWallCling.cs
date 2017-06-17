using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WallClingStatus
{
    bool isClingingToWall { get; }
}

//TODO(robert): what if player is not using rigidbody for physics simulation?
//remove reference to rigidbody.
[RequireComponent(typeof(WallStatus), typeof(PlayerControls),
    typeof(PlayerDirection))]
[RequireComponent(typeof(Rigidbody2D), typeof(EventController), typeof(GroundStatus))]
public class PlayerWallCling : MonoBehaviour, WallClingStatus
{
    [Range(0f, 1f)]
    public float velocityDampener = 0.05f;

    public bool isClingingToWall { get; private set; }

    private WallStatus wallStatus;
    private GroundStatus groundStatus;
    private PlayerControls playerControls;
    private PlayerDirection facing;
    private Rigidbody2D rigidbody2d;
    private PlayerWallClingEvent clingEvent;
    private EventController events;

    void Awake()
    {
        wallStatus = GetComponent<WallStatus>();
        groundStatus = GetComponent<GroundStatus>();
        playerControls = GetComponent<PlayerControls>();
        facing = GetComponent<PlayerDirection>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        events = GetComponent<EventController>();
    }

    void Start()
    {
        clingEvent = new PlayerWallClingEvent(this);
    }


    // Update is called once per frame
    void Update()
    {
        if(groundStatus.isGrounded)
        {
            if(isClingingToWall)
            {
                isClingingToWall = false;

                events.Raise(clingEvent);
            }

            return;
        }

        if (wallStatus.isTouchingWall)
        {
            if(isClingingToWall && playerControls.IsNoMovementControlPressed())
            {
                isClingingToWall = false;
                events.Raise(clingEvent);

                if(rigidbody2d.velocity.y < 0f)
                {
                    rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x,
                        rigidbody2d.velocity.y * (2f + velocityDampener));
                }
                return;
            }

            if(playerControls.IsNoMovementControlPressed())
            {
                return;
            }

            var move = playerControls.GetMovement();

            if ((facing.currentDirection == FacingDirection.Left && move < 0f) ||
                (facing.currentDirection == FacingDirection.Right && move > 0f))
            {
                bool prevStatus = isClingingToWall;

                isClingingToWall = true;
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y * velocityDampener);

                if (prevStatus != isClingingToWall)
                {
                    events.Raise(clingEvent);
                }
            }
        }
        else if (isClingingToWall)
        {
            isClingingToWall = false;
            events.Raise(clingEvent);

        }
    }
}
