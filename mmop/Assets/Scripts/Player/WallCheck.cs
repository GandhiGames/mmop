using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventController))]
public class WallCheck : MonoBehaviour
{
    public Transform[] forwardTransforms;
    public LayerMask wallMask;

    //TODO: normalise static variable naming.
    private static GameEvent TOUCHING_WALL_EVENT;
    private static GameEvent NOT_TOUCHING_WALL_EVENT;

    private EventController events;
    private bool wasPreviouslyTouchingWall = false;

    void Awake()
    {
        events = GetComponent<EventController>();
    }

    void Start()
    {
        if (TOUCHING_WALL_EVENT == null)
        {
            TOUCHING_WALL_EVENT = new PlayerWallStatusEventChangeEvent(true);
        }

        if (NOT_TOUCHING_WALL_EVENT == null)
        {
            NOT_TOUCHING_WALL_EVENT = new PlayerWallStatusEventChangeEvent(false);
        }
    }

    void Update()
    {
        bool touchingWall = false;
        foreach (var forward in forwardTransforms)
        {
            var origin = new Vector2(transform.position.x, forward.position.y);

            RaycastHit2D hit = Physics2D.Linecast(origin, forward.position, wallMask);

            if (hit.collider != null)
            {
                touchingWall = true;
                break;
            }
        }

        // Raise an event if the wall touching status has changed since last frame.
        if (touchingWall && !wasPreviouslyTouchingWall)
        {
            events.Raise(TOUCHING_WALL_EVENT);
        }
        else if (!touchingWall && wasPreviouslyTouchingWall)
        {
            events.Raise(NOT_TOUCHING_WALL_EVENT);
        }

        wasPreviouslyTouchingWall = touchingWall;
    }
}
