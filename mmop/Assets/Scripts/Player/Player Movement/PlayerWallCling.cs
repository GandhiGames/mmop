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
[RequireComponent(typeof(Rigidbody2D), typeof(EventController))]
public class PlayerWallCling : MonoBehaviour, WallClingStatus
{
    [Range(0f, 1f)]
    public float velocityDampener = 0.05f;

    public float downwardsForceWhenCrouching = 3f;

    public bool isClingingToWall { get; private set; }

    private WallStatus wallStatus;
    private PlayerControls playerControls;
    private PlayerDirection facing;
    private Rigidbody2D rigidbody2d;
    private PlayerWallClingEvent clingEvent;
    private EventController events;

    void Awake()
    {
        wallStatus = GetComponent<WallStatus>();
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
        if (wallStatus.isTouchingWall)
        {
            var move = playerControls.GetMovement();

            if ((facing.currentDirection == FacingDirection.Left && move < 0f) ||
                (facing.currentDirection == FacingDirection.Right && move > 0f))
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0f);


                isClingingToWall = true;
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y * velocityDampener);

                events.Raise(clingEvent);
            }
            else if(playerControls.IsCrouchButtonHeld())
            {
                rigidbody2d.AddForce(Vector2.down * downwardsForceWhenCrouching);
            }
        }
        else if (isClingingToWall)
        {
            isClingingToWall = false;
            events.Raise(clingEvent);

        }
    }
}
