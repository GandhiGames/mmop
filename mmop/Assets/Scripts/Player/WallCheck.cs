using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WallStatus
{
    bool isTouchingWall { get; }
}

public class WallCheck : MonoBehaviour, WallStatus
{
    public Transform[] forwardTransforms;
    public LayerMask wallMask;
    public bool isTouchingWall { get; set; }

    void Update()
    {
        foreach (var forward in forwardTransforms)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, forward.position, wallMask);

            if (hit.collider != null)
            {
                isTouchingWall = true;

                break;
            }
            else
            {
                isTouchingWall = false;
            }
        }
    }
}
