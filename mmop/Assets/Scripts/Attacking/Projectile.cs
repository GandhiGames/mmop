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
    private float reboundForce;
    private float maxTimeAlive;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();    
    }

    void Start()
    {
        Physics2D.queriesStartInColliders = false;    
    }

    void OnDisable()
    {
        damage = 0f;
        motor.velocity = Vector2.zero;
    }

    public void Initialise(EventController owner, 
        float damage, Vector2 initialMove, 
        float reboundForce, float maxTimeAlive)
    {
        this.owner = owner;
        this.damage = damage;
        this.reboundForce = reboundForce;
        this.maxTimeAlive = maxTimeAlive;

        motor.velocity = initialMove;

        float angle = Mathf.Atan2(motor.velocity.y, motor.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        print("do max time alive");
    }

    //TODO: set max rebound
    void OnTriggerEnter2D(Collider2D collision)
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
            else
            {
                motor.velocity = hit.normal * reboundForce;
                float angle = Mathf.Atan2(motor.velocity.y, motor.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

}
