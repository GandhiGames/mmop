using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CrouchStatus
{
    bool isCrouching { get; }
}

[RequireComponent(typeof(PlayerControls), typeof(EventController), typeof(GroundStatus))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCrouchController : MonoBehaviour, CrouchStatus
{
    public float downwardForceInAir = 3f;

    public bool isCrouching { get; private set; }

    private PlayerControls playerControls;
    private EventController eventController;
    private GroundStatus groundStatus;
    private Rigidbody2D rigidbody2d;
    private PlayerCrouchEvent crouchEvent;
    private bool previousCrouchStatus;

    void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        eventController = GetComponent<EventController>();
        groundStatus = GetComponent<GroundStatus>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        isCrouching = false;

        crouchEvent = new PlayerCrouchEvent(this);

        previousCrouchStatus = groundStatus.isGrounded;
    }

    // should be able to pass through traversable platform and still move in air.
    void Update()
    {
        bool shouldCrouch = playerControls.IsCrouchButtonHeld();

        if (shouldCrouch != isCrouching)
        {
            isCrouching = shouldCrouch;
            previousCrouchStatus = groundStatus.isGrounded;
            eventController.Raise(crouchEvent);
        }
        else if(isCrouching && previousCrouchStatus != groundStatus.isGrounded)
        {
            if((groundStatus.isGrounded && !groundStatus.ground.CompareTag("Platform")) || !groundStatus.isGrounded)
            {
                previousCrouchStatus = groundStatus.isGrounded;
                eventController.Raise(crouchEvent);
            }
        }

        if(isCrouching && !groundStatus.isGrounded)
        { 
            rigidbody2d.AddForce(Vector2.down * downwardForceInAir);
        }
    }
}
