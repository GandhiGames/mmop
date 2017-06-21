using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: remove player prefix from class names.
[RequireComponent(typeof(PlayerMotor), typeof(EventController))]
public class BounceUpWhenDownInAirMeleeAttack : MonoBehaviour
{
    public float upwardsForce = 100f;

    private EventController events;

    private PlayerMotor motor;
    private bool inAir = false;
    private bool isCrouched = false;

    void Awake()
    {
        events = GetComponent<EventController>();
        motor = GetComponent<PlayerMotor>();
    }

    void OnEnable()
    {
        events.AddListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChange);
        events.AddListener<PlayerCrouchEvent>(OnCrouchStatusChange);
        events.AddListener<DamageGivenEvent>(OnGivenDamage);
    }

    void OnDisable()
    {
        events.RemoveListener<PlayerGroundStatusChangeEvent>(OnGroundStatusChange);
        events.RemoveListener<PlayerCrouchEvent>(OnCrouchStatusChange);
        events.RemoveListener<DamageGivenEvent>(OnGivenDamage);
    }

    private void OnGroundStatusChange(PlayerGroundStatusChangeEvent e)
    {
        inAir = e.groundStatus == GroundStatus.NotGrounded;
    }

    private void OnCrouchStatusChange(PlayerCrouchEvent e)
    {
        isCrouched = e.status.isCrouching;
    }

    //TODO: look into delay from hit to force being added.
    // Could be due to animation call. Maybe remove method call from animation,
    // and have continous raycast when player attacking down.
    private void OnGivenDamage(DamageGivenEvent e)
    {
        // We can assume if the player isn't grounded and is in the air
        // and has just landed an attack (signified by the DamageGivenEvent),
        // that they have just performed a downards in air attack.
        if(inAir && isCrouched)
        {
            motor.AddForce(Vector2.up * upwardsForce);
        }
    }
}
