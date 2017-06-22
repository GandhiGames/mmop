using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: add sprite.
//TODO: add trail.
[RequireComponent(typeof(PlayerMotor))]
public class Projectile : MonoBehaviour
{
    public Transform hitTarget;

    public LayerMask hitMask;

    private PlayerMotor motor;
    private float damage;
    private EventController owner;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();    
    }

    void OnDisable()
    {
        damage = 0f;
        motor.velocity = Vector2.zero;
    }

    public void Initialise(EventController owner, float damage, Vector2 force)
    {
        this.owner = owner;
        this.damage = damage;

        motor.velocity = force;
    }

    //TODO: perform rebounds.
    void Update()
    {
        var hit = Physics2D.Linecast(transform.position, hitTarget.position, hitMask);

        if (hit.collider != null)
        {
            var otherController = hit.collider.GetComponent<EventController>();

            if (otherController != null)
            {
                Vector2 dir = (hitTarget.position - transform.position).normalized;

                otherController.Raise(new DamageTakenEvent(damage, dir));

                owner.Raise(new DamageGivenEvent(otherController));
            }
        }
    }

}
