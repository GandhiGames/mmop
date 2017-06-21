using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEvent : GameEvent
{
    public AttackStatus status { get; private set; }

    public PlayerAttackEvent(AttackStatus status)
    {
        this.status = status;
    }
}
