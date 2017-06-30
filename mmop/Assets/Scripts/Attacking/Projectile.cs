using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: projectiles still occassionaly go through obstacles.
[RequireComponent(typeof(PlayerMotor))]
public class Projectile : MonoBehaviour
{
    public Transform hitTarget;

    public LayerMask hitMask;

    public TrailRenderer trail;

    private PlayerMotor motor;
    private float damage;
    private ObjectPool<Projectile> owner;
    private float maxTimeAlive;
    private EventController events;
    private bool initialised = false;
    private float trailTime;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        trailTime = trail.time;
    }

    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    void OnDisable()
    {
        damage = 0f;
        motor.velocity = Vector2.zero;
        initialised = false;
        trail.time = 0f;
    }

    void OnEnable()
    {
        trail.time = trailTime;    
    }

    public void Initialise(ObjectPool<Projectile> owner, 
        EventController events,
        float damage, Vector2 initialMove, float maxTimeAlive)
    {
        this.owner = owner;
        this.events = events;
        this.damage = damage;
        this.maxTimeAlive = maxTimeAlive;

        motor.velocity = initialMove;

        float angle = Mathf.Atan2(motor.velocity.y, motor.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        initialised = true;
    }

    //TODO: fix arrows going through obstacles
    void Update()
    {
        if (initialised)
        {
            maxTimeAlive -= Time.deltaTime;

            if (maxTimeAlive <= 0f)
            {
                Remove();
            }
            
            float angle = Mathf.Atan2(motor.velocity.y, motor.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    //TODO: look into why this fixes projectile going through obstacles. Is there a better way of doing it?
    void FixedUpdate()
    {
        if(initialised)
        {
            CheckForTarget();
        }
    }


    //TODO: set max rebound
    private void Remove()
    {
        owner.Pool(this);
    }

    private void CheckForTarget()
    {
        var hit = Physics2D.Linecast(transform.position, hitTarget.position, hitMask);

        if (hit.collider != null)
        {
            // Do not attack player that shot projectile.
            if(hit.collider.transform.GetInstanceID() == events.transform.GetInstanceID())
            {
                return;
            }

            var otherController = hit.collider.GetComponent<EventController>();

            if (otherController != null)
            {
                Vector2 dir = (hitTarget.position - transform.position).normalized;

                otherController.Raise(new DamageTakenEvent(damage, dir));

                events.Raise(new DamageGivenEvent(otherController));

                Remove();
            }
            else
            {
                motor.velocity = Vector2.Reflect(motor.velocity, hit.normal);
            }
        }
    }
}
