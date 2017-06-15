using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventController))]
public class PlayerColliderController : MonoBehaviour
{
    public Collider2D[] colliders;

    private EventController eventController;

    void Awake()
    {
        eventController = GetComponent<EventController>();    
    }

    void OnEnable()
    {
        eventController.AddListener<PlayerColliderStatusChangeRequestEvent>(OnColliderStatusChangeRequest);    
    }

    void OnDisable()
    {
        eventController.RemoveListener<PlayerColliderStatusChangeRequestEvent>(OnColliderStatusChangeRequest);
    }

    private void OnColliderStatusChangeRequest(PlayerColliderStatusChangeRequestEvent e)
    {
        foreach(var c in colliders)
        {
            c.isTrigger = !e.shouldEnable;
        }
    }
}
