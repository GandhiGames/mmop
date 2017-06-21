using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControls), typeof(PlayerDirection))]
public class PlayerTeleportController : MovementAction
{
    public float teleportDistance = 1f;
    public float timeBetweenTeleports = 1f;
    public bool canTeleportThroughWalls = true;

    private static readonly float WALL_OFFSET = 0.1f;
    private static readonly float CLOSEST_POINT_STEP = 0.1f;

    private PlayerControls playerControls;
    private PlayerDirection facing;
    private float waitTime = 0f;

    protected override void Awake()
    {
        base.Awake();

        playerControls = GetComponent<PlayerControls>();
        facing = GetComponent<PlayerDirection>();
    }

    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        waitTime = timeBetweenTeleports;
    }

    void Update()
    {
        waitTime += Time.deltaTime;

        if (waitTime >= timeBetweenTeleports)
        {
            if (playerControls.IsMovementActionButtonPressed())
            {
                float teleportDir = facing.currentDirection == FacingDirection.Right ? 1f : -1f;

                var targetPos = transform.position + new Vector3(teleportDistance * teleportDir, 0f);

                var hits = Physics2D.RaycastAll(transform.position, new Vector2(teleportDir, 0f), teleportDistance);

                if (hits.Length == 0)
                {
                    transform.position = targetPos;
                }
                else
                {
                    //TODO(robert): calculate size of player sprite and move back from wall to prevent clipping when teleporting.
                    if (canTeleportThroughWalls)
                    {
                        if (!Physics2D.OverlapPoint(targetPos))
                        {
                            transform.position = targetPos;
                        }
                        else
                        {
                            hits.OrderBy(hit => hit.distance);

                            for (int i = hits.Length - 1; i >= 0; i--)
                            {
                                if (!Physics2D.OverlapPoint(hits[i].point + hits[i].normal * WALL_OFFSET))
                                {
                                    var point = hits[i].point;
                                    while(Physics2D.OverlapPoint(point))
                                    {
                                        point += CLOSEST_POINT_STEP * Vector2.right * -hits[i].normal.x;
                                    }

                                    var backwardsHit = Physics2D.Raycast(point, hits[i].normal);
                                    point = backwardsHit.point;

                                    if(Vector2.Distance(point, targetPos) < Vector2.Distance(hits[i].point, targetPos))
                                    {
                                        transform.position = (Vector3)(point + -hits[i].normal * WALL_OFFSET);
                                    }
                                    else
                                    {
                                        transform.position = (Vector3)(hits[i].point + hits[i].normal * WALL_OFFSET);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        var closest = hits.GetClosest();
                        transform.position = closest.point + closest.normal * WALL_OFFSET;
                    }
                }
                

                waitTime = 0f;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(teleportDistance * transform.localScale.x, 0f));
    }
}
