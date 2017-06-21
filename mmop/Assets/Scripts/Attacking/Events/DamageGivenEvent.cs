using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGivenEvent : GameEvent
{
    public EventController enemyEvents { get; private set; }

    public DamageGivenEvent(EventController enemyEvents)
    {
        this.enemyEvents = enemyEvents;
    }
}
