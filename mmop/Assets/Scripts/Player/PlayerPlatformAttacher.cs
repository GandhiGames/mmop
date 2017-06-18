using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GroundStatus))]
public class PlayerPlatformAttacher : MonoBehaviour
{
    private GroundStatus groundStatus;
    private GameObject platform;

    void Awake()
    {
        groundStatus = GetComponent<GroundStatus>(); 
    }

    void Update()
    {
        if(groundStatus.isGrounded && groundStatus.ground.CompareTag("Platform"))
        {
            if (groundStatus.ground.GetComponent<MovingPlatform>() != null)
            {
                if (platform == null || groundStatus.ground.GetInstanceID() != platform.GetInstanceID())
                {
                    platform = groundStatus.ground;
                    transform.SetParent(platform.transform);
                }
            }
        }
        else
        {
            platform = null;
            transform.SetParent(null);
        }
    }
}
