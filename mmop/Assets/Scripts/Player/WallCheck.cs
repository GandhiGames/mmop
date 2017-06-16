using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WallStatus
{
    bool isTouchingWall { get; }
}

[RequireComponent(typeof(PlayerControls))]
public class WallCheck : MonoBehaviour, WallStatus
{
    public Transform forwardTransform;
    public LayerMask wallMask;
    public bool isTouchingWall { get; set; }

    private PlayerControls controls;

    void Awake()
    {
        controls = GetComponent<PlayerControls>();    
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, forwardTransform.position, wallMask);

        if(hit.collider != null)
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall = false;
        }
    }
}
