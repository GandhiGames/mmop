using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerControls), 
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

    private Rigidbody2D rigidbody2d;
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
        rigidbody2d = GetComponent<Rigidbody2D>();
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
    }

    void FixedUpdate()
    {
        if (horizontalMovement == 0f || !canMove)
        {
            return;
        }

        float moveForce = groundStatus.isGrounded ? moveForceOnGround : moveForceInAir;

        rigidbody2d.velocity = new Vector2(horizontalMovement * moveForce * speedMultiplier, rigidbody2d.velocity.y);

        //rigidbody2d.AddForce(new Vector2(horizontalMovement * moveForce * speedMultiplier, 0f));

        if (Mathf.Abs(rigidbody2d.velocity.x) > maxSpeed)
        {
            rigidbody2d.velocity = new Vector2(Mathf.Sign(rigidbody2d.velocity.x) * maxSpeed, rigidbody2d.velocity.y);
        }

        direction.Face(horizontalMovement > 0f ? FacingDirection.Right : FacingDirection.Left);
    }

    private void PlayerCrouchStatusChanged(PlayerCrouchEvent e)
    {
        UpdateMovementStatus(!e.status.isCrouching);
    }

    private void PlayerAttackStatusChanged(PlayerAttackEvent e)
    {
        UpdateMovementStatus(!e.status.isAttackingPrimary && !e.status.isAttackingSecondary);
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
        if(pausingMovement)
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
