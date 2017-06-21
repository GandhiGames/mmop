using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTakenEvent : GameEvent
{
    public float damage { get; private set; }
    public Vector2 direction { get; private set; }

    public DamageTakenEvent(float damage, Vector2 direction)
    {
        this.damage = damage;
        this.direction = direction;
    }
}
