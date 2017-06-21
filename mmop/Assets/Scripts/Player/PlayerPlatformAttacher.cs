using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventController))]
public class PlayerPlatformAttacher : MonoBehaviour
{
    private EventController events;

    void Awake()
    {
        events = GetComponent<EventController>();
    }

    void OnEnable()
    {
        events.AddListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
    }

    void OnDisable()
    {
        events.RemoveListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChanged);
    }

    private void OnGroundStatusChanged(PlayerGroundStatusChangeEvent e)
    {
        // If we are grounded on a platform.
        if (e.groundStatus == GroundStatus.Grounded &&
            e.ground.CompareTag("Platform"))
        {
            // If the platform is a moving platform.
            if (e.ground.GetComponent<MovingPlatform>() != null)
            {
                // We now know we are standing on a moving platform,
                // so we set it to be our parent, ensuring as the platform moves
                // in the world so do we.
                transform.SetParent(e.ground.transform);
            }
        }
        else
        {
            // We are not standing on a platform so we want to remove any parents we have.
            transform.SetParent(null);
        }
    }
}
