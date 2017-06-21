using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RunStatus
{
    bool shouldRun { get; }
    float runMultiplier { get; }
}

[RequireComponent(typeof(PlayerControls), typeof(EventController))]
public class PlayerRunController : MovementAction, RunStatus
{
    public float runMoveMultiplier = 1.4f;
    public float runMultiplier { get { return runMoveMultiplier; } }
    public bool shouldRun { get; private set; }

    private PlayerControls playerControls;
    private PlayerRunEvent runEvent;
    private EventController eventController;

    protected override void Awake()
    {
        base.Awake();

        eventController = GetComponent<EventController>();
    }

    void Start()
    {
        runEvent = new PlayerRunEvent(this);
    }

    void Update()
    {
        bool lastRunStatus = shouldRun;

        if (playerControls.IsMovementActionButtonPressed())
        {
            shouldRun = true;
        }
        else if(playerControls.IsMovementActionButtonReleased())
        {
            shouldRun = false;
        }

        if(shouldRun != lastRunStatus)
        {
            eventController.Raise(runEvent);
        }
    }
}
