using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CrouchStatus
{
    bool isCrouching { get; }
}

//TODO: player should press crouch and jump button to drop down from platform
// or should have to jump and then press crouch.
[RequireComponent(typeof(PlayerControls), typeof(EventController))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerCrouchController : MonoBehaviour, CrouchStatus
{
    public float downwardForceInAir = 3f;

    public bool isCrouching { get; private set; }

    private PlayerControls playerControls;
    private EventController eventController;
    private PlayerMotor motor;
    private PlayerCrouchEvent crouchEvent;
    private bool shouldAddDownwardForce = false;

    void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        eventController = GetComponent<EventController>();
        motor = GetComponent<PlayerMotor>();
    }

    void Start()
    {
        isCrouching = false;

        crouchEvent = new PlayerCrouchEvent(this);
    }

    void OnEnable()
    {
        eventController.AddListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
        eventController.AddListener<PlayerAttackEvent>(OnAttackStatusChanged);
    }

    void OnDisable()
    {
        eventController.RemoveListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
        eventController.RemoveListener<PlayerAttackEvent>(OnAttackStatusChanged);
    }

    //TODO: should be able to pass through traversable platform while crouch attacking.
    void Update()
    {
        bool shouldCrouch = playerControls.IsCrouchButtonHeld();

        if (shouldCrouch != isCrouching)
        {
            // Because our crouch status has changed we need to raise a new event,
            // letting anyone interested know.
            isCrouching = shouldCrouch;
            eventController.Raise(crouchEvent);
        }
    }

    private void FixedUpdate()
    {
        // If we crouch in the air we want to move the player downwards quicker.
        if(isCrouching && shouldAddDownwardForce)
        {
            motor.AddForce(Vector2.down * downwardForceInAir);
        }
    }

    
    private void OnGroundStatusChanged(PlayerGroundStatusChangeEvent e)
    {
        // If we set shouldAddDownwardForce to true last event then we know
        // we were in the air and vise versa. This can be used as an indication
        // of our previous ground status.
        bool previousGrounded = !shouldAddDownwardForce;
        bool grounded = e.groundStatus == GroundStatus.Grounded;

        shouldAddDownwardForce = !grounded;

        // If we are crouching and previously we were not touching 
        // the ground but now we are
        if (isCrouching && grounded && !previousGrounded)
        {
            if (!e.ground.CompareTag("Platform"))
            {
                // Raise event to let other classes know that while we are still crouching,
                // we have moved from in air to ground.
                eventController.Raise(crouchEvent);
            }
        }
    }

    private void OnAttackStatusChanged(PlayerAttackEvent e)
    {
        shouldAddDownwardForce = e.status.attackType != PlayerAttackType.Primary;
    }
}
