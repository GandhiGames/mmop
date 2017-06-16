using System;
using System.Collections.Generic;
using UnityEngine;

public interface GroundStatus
{
    bool isGrounded { get; }
    bool isAlmostGrounded { get; }
    GameObject ground { get; }
}

[RequireComponent(typeof(Rigidbody2D))]
public class GroundCheck : MonoBehaviour, GroundStatus
{
    public Transform groundTransform;
    public Transform almostGroundTransform;
    public LayerMask platformLayer;

    public bool isGrounded { get; private set; }
    public bool isAlmostGrounded { get; private set; }
    public GameObject ground { get; private set; }

    private Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        //TODO(robert): what if character is not using rigidbody? Generalise based
        //on jumping system.
        if (rigidbody2d.velocity.y > 0f)
        {
            isGrounded = false;
            isAlmostGrounded = false;
            ground = null;

            return;
        }
 
        if(rigidbody2d.velocity.y < 0f && !isAlmostGrounded)
        {
            RaycastHit2D almostGroundHit = Physics2D.Linecast(transform.position, almostGroundTransform.position, platformLayer);

            if (almostGroundHit.collider != null)
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, groundTransform.position, platformLayer);

                if (hit.collider == null)
                {
                    isAlmostGrounded = true;
                    isGrounded = false;
                    ground = almostGroundHit.collider.gameObject;

                    return;
                }
            }
        }

        RaycastHit2D groundHit = Physics2D.Linecast(transform.position, groundTransform.position, platformLayer);

        if (groundHit.collider != null)
        {
            isAlmostGrounded = false;
            isGrounded = true;
            ground = groundHit.collider.gameObject;
        }
        else
        {
            isAlmostGrounded = false;
            isGrounded = false;
            ground = null;
        }
    }
}
