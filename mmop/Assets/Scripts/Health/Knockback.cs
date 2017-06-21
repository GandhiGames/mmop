using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor), typeof(EventController))]
public class Knockback : MonoBehaviour
{
    public float knockbackForce = 100f;

    private PlayerMotor motor;
    private EventController eventController;

    void Awake()
    {
        motor = GetComponent<PlayerMotor>();
        eventController = GetComponent<EventController>();
    }

    void OnEnable()
    {
        eventController.AddListener<DamageTakenEvent>(OnDamageTaken);
    }

    void OnDisable()
    {
        eventController.RemoveListener<DamageTakenEvent>(OnDamageTaken);
    }

    private void OnDamageTaken(DamageTakenEvent e)
    {
        motor.AddForce(e.direction * knockbackForce);
    }

}
