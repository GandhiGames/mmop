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

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        playerControls = GetComponent<PlayerControls>();
        groundStatus = GetComponent<GroundStatus>();
        direction = GetComponent<PlayerDirection>();
        eventController = GetComponent<EventController>();
    }

    void OnEnable()
    {
        eventController.AddListener<PlayerCrouchEvent>(PlayerCrouchStatusChanged);
        eventController.AddListener<PlayerAttackEvent>(PlayerAttackStatusChanged);
        eventController.AddListener<PlayerRunEvent>(PlayerRunStatusChanged);
        eventController.AddListener<WallJumpEvent>(OnPlayerWallJumped);
    }

    void OnDisable()
    {
        eventController.RemoveListener<PlayerCrouchEvent>(PlayerCrouchStatusChanged);
        eventController.RemoveListener<PlayerAttackEvent>(PlayerAttackStatusChanged);
        eventController.RemoveListener<PlayerRunEvent>(PlayerRunStatusChanged);
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
            if (instantStopGround && groundStatus.isGrounded)
            {
                motor.velocity = new Vector2(0f, motor.velocity.y);
                horizontalMovement = 0f;
            }
            else if (instantStopAir && !groundStatus.isGrounded)
            {
                motor.velocity = new Vector2(0f, motor.velocity.y);
                horizontalMovement = 0f;
            }
        }
    }

    //TODO: max speed check is not performed if there is no input.
    // Should check y velocity as well.
    void FixedUpdate()
    {
        if (horizontalMovement == 0f)
        {
            return;
        }

        if (!canMove)
        {
            return;
        }

        float moveForce = groundStatus.isGrounded ? moveForceOnGround : moveForceInAir;

        if (horizontalMovement < 0f && direction.currentDirection == FacingDirection.Right
            || horizontalMovement > 0f && direction.currentDirection == FacingDirection.Left)
        {
            if (Mathf.Abs(motor.velocity.x) >= maxSpeed * 0.2f)
            {
                print("Reactive direction change");
                moveForce += moveForce * reactivityPercentage;
            }
        }

        motor.velocity = new Vector2(horizontalMovement * moveForce * speedMultiplier, motor.velocity.y);

        //rigidbody2d.AddForce(new Vector2(horizontalMovement * moveForce * speedMultiplier, 0f));

        if (Mathf.Abs(motor.velocity.x) > maxSpeed)
        {
            print("Max speed reached");
            motor.velocity = new Vector2(Mathf.Sign(motor.velocity.x) * maxSpeed, motor.velocity.y);
        }

        direction.Face(horizontalMovement > 0f ? FacingDirection.Right : FacingDirection.Left);
    }

    //TODO: should be able to move whilst crouched and in the air,
    // and should not be able to move when touch ground.
    private void PlayerCrouchStatusChanged(PlayerCrouchEvent e)
    {
        if (groundStatus.isGrounded)
        {
            UpdateMovementStatus(!e.status.isCrouching);

            if(e.status.isCrouching)
            {
                motor.velocity = new Vector2(0f, motor.velocity.y);
            }
        }
    }

    //TODO: player should still be able to move when attacking in air.
    // should be player always be able to move while attacking? Possibly
    // reduce speed while attacking.
    private void PlayerAttackStatusChanged(PlayerAttackEvent e)
    {
        //UpdateMovementStatus(!e.status.isAttackingPrimary && !e.status.isAttackingSecondary);
    }

    private void PlayerRunStatusChanged(PlayerRunEvent e)
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
