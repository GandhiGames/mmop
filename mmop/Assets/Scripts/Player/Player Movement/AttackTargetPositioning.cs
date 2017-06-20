using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTargetPositioning : MonoBehaviour
{
    public Vector2 standingPosition;
    public Vector2 croucingPosition;

    private EventController eventController;

    void Awake()
    {
        eventController = transform.parent.GetComponent<EventController>();    
    }

    void OnEnable()
    {
        eventController.AddListener<PlayerCrouchEvent>(OnCrouchStatusChanged);    
    }

    void OnDisable()
    {
        eventController.RemoveListener<PlayerCrouchEvent>(OnCrouchStatusChanged);
    }

    private void OnCrouchStatusChanged(PlayerCrouchEvent e)
    {
        if(e.status.isCrouching)
        {
            transform.localPosition = croucingPosition;
        }
        else
        {
            transform.localPosition = standingPosition;
        }
    }
}
