using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventController))]
public class MeleeAttack : MonoBehaviour
{
    public float damage = 1f;
    public LayerMask hitMask;

    public Transform hitTarget;

    private EventController events;

    void Awake()
    {
        events = GetComponent<EventController>();    
    }

    void Start()
    {
        Physics2D.queriesStartInColliders = false;    
    }

    public void PerformMeleeAttack()
    { 
        var hit = Physics2D.Linecast(transform.position, hitTarget.position, hitMask);

        if(hit.collider != null)
        { 
            var otherController = hit.collider.GetComponent<EventController>();

            if (otherController != null)
            {
                Vector2 dir = (hitTarget.position - transform.position).normalized;

                otherController.Raise(new DamageTakenEvent(damage, dir));

                events.Raise(new DamageGivenEvent(otherController));
            }
        }
    }
}