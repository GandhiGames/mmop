using System;
using System.Collections.Generic;
using UnityEngine;

public interface GroundStatus
{
    bool isGrounded { get; }
    bool isAlmostGrounded { get; }
    GameObject ground { get; }
}

//TODO: turn this into event rather than polling.
[RequireComponent(typeof(PlayerMotor))]
public class GroundCheck : MonoBehaviour, GroundStatus
{
    public Transform[] groundTransforms;
    public Transform almostGroundTransform;
    public LayerMask platformLayer;

    public bool isGrounded { get; private set; }
    public bool isAlmostGrounded { get; private set; }
    public GameObject ground { get; private set; }

    private PlayerMotor motor;

    //TODO: should ground check be aware of animator? update this when moved over to event system.
    private Animator animator;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //TODO: what if character is not using rigidbody? Generalise based
        //on jumping system.
        if (motor.velocity.y > 0f)
        {
            isGrounded = false;
            isAlmostGrounded = false;
            ground = null;

            animator.SetBool("grounded", isGrounded);

            return;
        }
 
        if(motor.velocity.y < 0f && !isAlmostGrounded)
        {
            RaycastHit2D almostGroundHit = Physics2D.Linecast(transform.position, almostGroundTransform.position, platformLayer);

            if (almostGroundHit.collider != null)
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, groundTransforms[0].position, platformLayer);

                if (hit.collider == null)
                {
                    isAlmostGrounded = true;
                    isGrounded = false;
                    ground = almostGroundHit.collider.gameObject;

                    animator.SetBool("grounded", isGrounded);

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
                isAlmostGrounded = false;
                isGrounded = true;
                ground = groundHit.collider.gameObject;

                break;
            }
        }

        if(!isGrounded)
        {
            isAlmostGrounded = false;
            isGrounded = false;
            ground = null;
        }

        animator.SetBool("grounded", isGrounded);
    }
}
