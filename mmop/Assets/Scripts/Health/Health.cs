using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EventController))]
public class Health : MonoBehaviour
{
    public float hitPoints;

    private float currentHitPoints;
    private EventController eventController;

    void Awake()
    {
        eventController = GetComponent<EventController>();    
    }

    void OnEnable()
    {
        currentHitPoints = hitPoints;

        eventController.AddListener<DamageTakenEvent>(OnDamageTaken);
    }

    void OnDisable()
    {
        eventController.RemoveListener<DamageTakenEvent>(OnDamageTaken);
    }

    private void OnDamageTaken(DamageTakenEvent e)
    {
        currentHitPoints -= e.damage;

        if(currentHitPoints <= 0)
        {
            currentHitPoints = 0f;

            OnDeath();
        }

        print("Damage Taken: " + e.damage + " current health: " + currentHitPoints);
    }

    private void OnDeath()
    {
        print("Entity killed");
    }
}
