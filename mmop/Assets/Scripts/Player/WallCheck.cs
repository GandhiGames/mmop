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
            var origin = new Vector2(transform.position.x, forward.position.y);

            RaycastHit2D hit = Physics2D.Linecast(origin, forward.position, wallMask);

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
