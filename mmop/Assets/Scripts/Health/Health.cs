using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, Damageable
{
    public float hitPoints;

    private float currentHitPoints;

    void Start()
    {
        currentHitPoints = hitPoints;    
    }

    public void Damage(float amount, Vector2 dir)
    {
        currentHitPoints-=amount;

        if(currentHitPoints <= 0)
        {
            currentHitPoints = 0f;

            OnDeath();
        }

        print("Damage Taken: " + amount + " current health: " + currentHitPoints);
    }

    private void OnDeath()
    {
        print("Entity killed");
    }
}
