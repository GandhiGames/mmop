using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerMotor), typeof(EventController))]
public class GroundCheck : MonoBehaviour
{
    public Transform[] groundTransforms;
    public Transform almostGroundTransform;
    public LayerMask platformLayer;

    private GroundStatus previousStatus;
    private int previousGroundId;
    private PlayerMotor motor;
    private EventController eventController;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        eventController = GetComponent<EventController>();
    }

    void Start()
    {
        previousStatus = GroundStatus.None;
        previousGroundId = Int32.MinValue;
    }

    void Update()
    {
        if (motor.velocity.y > 0f)
        {
            RegisterState(GroundStatus.NotGrounded);
            return;
        }

        if (motor.velocity.y < 0f && previousStatus != GroundStatus.AlmostGrounded)
        {
            RaycastHit2D almostGroundHit = Physics2D.Linecast(transform.position, almostGroundTransform.position, platformLayer);

            if (almostGroundHit.collider != null)
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, groundTransforms[0].position, platformLayer);

                if (hit.collider == null)
                {
                    RegisterState(GroundStatus.AlmostGrounded, almostGroundHit.collider.gameObject);

                    return;
                }
            }
        }

        foreach (var groundTransform in groundTransforms)
        {
            var origin = new Vector2(groundTransform.position.x, transform.position.y);

            RaycastHit2D groundHit = Physics2D.Linecast(origin, groundTransform.position, platformLayer);

            Debug.DrawLine(origin, groundTransform.position);

            if (groundHit.collider != null)
            {
                RegisterState(GroundStatus.Grounded, groundHit.collider.gameObject);

                return;
            }
        }

        // If the player is not grounded for reasons than jumping (for example they walked off
        // a platform, then we end up here. We make sure that we are not almost about
        // to hit the ground because the only other way we would reach this point is if
        // we were almost grounded the previous frame and still are this frame.
        if (previousStatus != GroundStatus.AlmostGrounded)
        {
            RegisterState(GroundStatus.NotGrounded);
        }
    }

    private void RegisterState(GroundStatus status, GameObject ground = null)
    {
        int id = ground != null ? ground.GetInstanceID() : previousGroundId;

        // Raise event if either ground status or grounded object changes.
        if (status != previousStatus || id != previousGroundId)
        {
            previousStatus = status;
            previousGroundId = id;

            eventController.Raise(new PlayerGroundStatusChangeEvent(status, ground));
        }
    }
}
