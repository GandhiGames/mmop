using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WallJumpStatus
{
    FacingDirection directionToJump { get; }
    float horizontalForce { get; }
}

[RequireComponent(typeof(GroundStatus), typeof(PlayerControls), typeof(EventController))]
public class PlayerWallJump : MonoBehaviour, WallJumpStatus
{
    public Transform forward;

    public LayerMask wallJumpPlayer;

    public float wallJumpHorizontalForce = 75f;
    public float horizontalForce { get { return wallJumpHorizontalForce; } }
    public FacingDirection directionToJump { get; private set; }

    private GroundStatus groundStatus;
    private PlayerControls playerControls;
    private WallJumpEvent jumpEvent;
    private EventController eventController;

    void Awake()
    {
        groundStatus = GetComponent<GroundStatus>();
        playerControls = GetComponent<PlayerControls>();
        eventController = GetComponent<EventController>();
    }

    void Start()
    {
        jumpEvent = new WallJumpEvent(this);    
    }

    void Update()
    {
        if(groundStatus.isGrounded)
        {
            return;
        }

        if (playerControls.IsJumpButtonPressed())
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, forward.position, wallJumpPlayer);

            if (hit.collider != null)
            {              
                directionToJump = hit.collider.transform.position.x > transform.position.x ? FacingDirection.Left  : FacingDirection.Right;

                eventController.Raise(jumpEvent);
            }
        }
    }
}
