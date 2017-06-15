using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CrouchStatus
{
    bool isCrouching { get; }
}

[RequireComponent(typeof(PlayerControls), typeof(EventController))]
public class PlayerCrouchController : MonoBehaviour, CrouchStatus
{
    public bool isCrouching { get; private set; }

    private PlayerControls playerControls;
    private EventController eventController;

    void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        eventController = GetComponent<EventController>();
    }

    void Start()
    {
        isCrouching = false;
    }

    void Update()
    {
        bool shouldCrouch = playerControls.IsCrouchButtonHeld();

        if (shouldCrouch != isCrouching)
        {
            isCrouching = shouldCrouch;
            eventController.Raise(new PlayerCrouchEvent(this));
        }
    }
}
