using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor), typeof(PlayerControls),
    typeof(PlayerDirection))]
[RequireComponent(typeof(EventController))]
public class PlayerMovementController : MonoBehaviour
{
    public float moveForceOnGround = 20f;
    public float moveForceInAir = 10f;
    public float maxSpeed = 2f;

    /// <summary>
    /// Movement force is multiplied by this when running.
    /// </summary>
    public float runForceMultiplier = 1.2f;

    [Range(0f, 1f)]
    public float reactivityPercentage = 0.5f;

    public bool instantStopGround = true;
    public bool instantStopAir = false;

    private PlayerMotor motor;
    private PlayerControls playerControls;
    private float horizontalMovement = 0f;
    private bool canMove = true;
    private GroundStatus groundStatus;
    private float speedMultiplier = 1f;
    private PlayerDirection direction;
    private bool pausingMovement = false;
    private EventController eventController;
    private float moveForce;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        playerControls = GetComponent<PlayerControls>();
        direction = GetComponent<PlayerDirection>();
        eventController = GetComponent<EventController>();
    }

    void Start()
    {
        groundStatus = GroundStatus.None;
        moveForce = moveForceOnGround;
    }

    void OnEnable()
    {
        eventController.AddListener<PlayerCrouchEvent>(OnCrouchStatusChanged);
        eventController.AddListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
        eventController.AddListener<PlayerRunEvent>(OnRunStatusChanged);
        eventController.AddListener<WallJumpEvent>(OnPlayerWallJumped);
    }

    void OnDisable()
    {
        eventController.RemoveListener<PlayerCrouchEvent>(OnCrouchStatusChanged);
        eventController.RemoveListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
        eventController.RemoveListener<PlayerRunEvent>(OnRunStatusChanged);
        eventController.RemoveListener<WallJumpEvent>(OnPlayerWallJumped);
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        horizontalMovement = playerControls.GetMovement();

        if (playerControls.IsNoMovementControlPressed())
        {
            if (instantStopGround && groundStatus == GroundStatus.Grounded)
            {
                motor.velocity = new Vector2(0f, motor.velocity.y);
                horizontalMovement = 0f;
            }
            else if (instantStopAir && groundStatus != GroundStatus.Grounded)
            {
                motor.velocity = new Vector2(0f, motor.velocity.y);
                horizontalMovement = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (horizontalMovement == 0f)
        {
            // Even if there is no input we want to clamp the players max speed to 
            // limit force applied by external forces.
            ClampMaxSpeed();

            return;
        }

        if (!canMove)
        {
            return;
        }

        // If we were facing in one direction and now we are facing the opposite direction.
        if (horizontalMovement < 0f && direction.currentDirection == FacingDirection.Right
            || horizontalMovement > 0f && direction.currentDirection == FacingDirection.Left)
        {
            // If we are moving at a percentage of our maximum speed.
            if (Mathf.Abs(motor.velocity.x) >= maxSpeed * 0.2f)
            {
                // Increase move force when turning directions. This provides
                // a turn that is quicker than it would have been otherwise, which feels
                // more reactive to the player.
                moveForce += moveForce * reactivityPercentage;
            }
        }

        motor.velocity = new Vector2(horizontalMovement * moveForce * speedMultiplier, motor.velocity.y);

        ClampMaxSpeed();

        direction.Face(horizontalMovement > 0f ? FacingDirection.Right : FacingDirection.Left);
    }

    private void ClampMaxSpeed()
    {
        if (Mathf.Abs(motor.velocity.x) > maxSpeed)
        {
            print("Max speed reached");
            motor.velocity = new Vector2(Mathf.Sign(motor.velocity.x) * maxSpeed, motor.velocity.y);
        }
    }

    private void OnCrouchStatusChanged(PlayerCrouchEvent e)
    {
        if (groundStatus == GroundStatus.Grounded)
        {
            // If we are grounded and crouching we do not want the player
            // to be able to move.
            UpdateMovementStatus(!e.status.isCrouching);

            if (e.status.isCrouching)
            {
                motor.velocity = new Vector2(0f, motor.velocity.y);
            }
        }
    }

    private void OnGroundStatusChanged(PlayerGroundStatusChangeEvent e)
    {
        groundStatus = e.groundStatus;

        moveForce = groundStatus == GroundStatus.Grounded ? moveForceOnGround : moveForceInAir;

        // If the player is not touching the ground we want them to be able to
        // override anything that says it cannot move.
        if (groundStatus == GroundStatus.NotGrounded)
        {
            UpdateMovementStatus(true);
        }

    }

    private void OnRunStatusChanged(PlayerRunEvent e)
    {
        speedMultiplier = e.status.shouldRun ? e.status.runMultiplier : 1f;
    }

    private void OnPlayerWallJumped(WallJumpEvent e)
    {
        StartCoroutine(PauseMovement(0.1f));
    }

    private void UpdateMovementStatus(bool move)
    {
        if (pausingMovement)
        {
            return;
        }

        canMove = move;

        if (!canMove)
        {
            horizontalMovement = 0f;
        }
    }

    private IEnumerator PauseMovement(float seconds)
    {
        canMove = false;
        pausingMovement = true;

        yield return new WaitForSeconds(seconds);

        pausingMovement = false;
        canMove = true;

    }
}
