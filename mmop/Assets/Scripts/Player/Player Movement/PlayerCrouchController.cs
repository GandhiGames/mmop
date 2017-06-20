﻿using System.Collections;
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

    //TODO: generalise to movement controller interface.
    private Rigidbody2D rigidbody2d;
    private PlayerCrouchEvent crouchEvent;
    private bool previousCrouchStatus;
    private bool shouldAddDownwardForce = true;

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

    void OnEnable()
    {
        eventController.AddListener<PlayerAttackEvent>(OnPlayerAttackStatusChanged);
    }

    void OnDisable()
    {
        eventController.RemoveListener<PlayerAttackEvent>(OnPlayerAttackStatusChanged);
    }

    //TODO: should be able to pass through traversable platform while crouch attacking.
    void Update()
    {
        bool shouldCrouch = playerControls.IsCrouchButtonHeld();

        if (shouldCrouch != isCrouching)
        {
            isCrouching = shouldCrouch;
            previousCrouchStatus = groundStatus.isGrounded;
            eventController.Raise(crouchEvent);
        }
        else if (isCrouching && previousCrouchStatus != groundStatus.isGrounded)
        {
            if ((groundStatus.isGrounded && !groundStatus.ground.CompareTag("Platform")) || !groundStatus.isGrounded)
            {
                previousCrouchStatus = groundStatus.isGrounded;
                eventController.Raise(crouchEvent);
            }
        }

        if (isCrouching && shouldAddDownwardForce && !groundStatus.isGrounded)
        {
            rigidbody2d.AddForce(Vector2.down * downwardForceInAir);
        }
    }

    private void OnPlayerAttackStatusChanged(PlayerAttackEvent e)
    {
        shouldAddDownwardForce = e.status.attackType != PlayerAttackType.Primary;
    }
}
