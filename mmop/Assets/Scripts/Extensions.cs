using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions 
{
    public static RaycastHit2D GetClosest(this RaycastHit2D[] hits)
    {
        Debug.Assert(hits.Length > 0);

        RaycastHit2D closest = new RaycastHit2D();
        float dist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.distance < dist)
            {
                closest = hit;
                dist = hit.distance;
            }
        }

        return closest;
    }

    public static RaycastHit2D GetFurthest(this RaycastHit2D[] hits)
    {
        Debug.Assert(hits.Length > 0);

        RaycastHit2D furthest = new RaycastHit2D();
        float dist = 0f;

        foreach (var hit in hits)
        {
            if (hit.distance > dist)
            {
                furthest = hit;
                dist = hit.distance;
            }
        }

        return furthest;
    }


}
