using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RunStatus
{
    bool shouldRun { get; }
    float runMultiplier { get; }
}

public interface MovementAction
{

}

[RequireComponent(typeof(PlayerControls), typeof(EventController))]
public class PlayerRunController : MonoBehaviour, RunStatus, MovementAction
{
    public float runMoveMultiplier = 1.4f;
    public float runMultiplier { get { return runMoveMultiplier; } }
    public bool shouldRun { get; private set; }

    private PlayerControls playerControls;
    private PlayerRunEvent runEvent;
    private EventController eventController;

    void Awake()
    {
        playerControls = GetComponent<PlayerControls>();

        var otherMoveActions = GetComponents<MovementAction>();

        if(otherMoveActions.Length > 1)
        {
            Debug.LogWarning("Only one movement action should be attached to a character");
        }

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
