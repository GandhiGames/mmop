using System;
using System.Collections.Generic;
using UnityEngine;

public interface GroundStatus
{
    bool isGrounded { get; }
    GameObject ground { get; }
}

[RequireComponent(typeof(Rigidbody2D))]
public class GroundCheck : MonoBehaviour, GroundStatus
{
    public Transform downTransform;
    public LayerMask platformLayer;

    public bool isGrounded { get; private set; }
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
        if (Mathf.Abs(rigidbody2d.velocity.y) > 0f)
        {
            isGrounded = false;
            ground = null;

            return;
        }

        RaycastHit2D hit = Physics2D.Linecast(transform.position, downTransform.position, platformLayer);

        if (hit.collider != null)
        {
            isGrounded = true;
            ground = hit.collider.gameObject;
        }
        else
        {
            isGrounded = false;
            ground = null;
        }
    }
}
